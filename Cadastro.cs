using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

public class Cadastro
{
private List<Usuario> usuarios = new List<Usuario>();
private List<Ambiente> ambientes = new List<Ambiente>();


// ======================= USUÁRIOS =======================  
public void AdicionarUsuario(Usuario u) => usuarios.Add(u);  

public Usuario? PesquisarUsuario(int id) => usuarios.Find(u => u.Id == id);  

public void RemoverUsuario(Usuario u) => usuarios.Remove(u);  

// ======================= AMBIENTES =======================  
public void AdicionarAmbiente(Ambiente a) => ambientes.Add(a);  

public Ambiente? PesquisarAmbiente(int id) => ambientes.Find(a => a.Id == id);  

public void RemoverAmbiente(Ambiente a) => ambientes.Remove(a);  

// ======================= LOGS DE ACESSO =======================  
public void RegistrarAcesso(int usuarioId, int ambienteId)  
{  
    bool autorizado = VerificarPermissao(usuarioId, ambienteId);  
    SalvarLog(new Log(usuarioId, ambienteId, autorizado));  
}  

private bool VerificarPermissao(int usuarioId, int ambienteId)  
{  
    using (var conn = Database.GetConnection())  
    {  
        conn.Open();  
        var cmd = new SqlCommand(  
            "SELECT COUNT(*) FROM UsuarioAmbiente WHERE UsuarioId=@U AND AmbienteId=@A",  
            conn);  

        cmd.Parameters.AddWithValue("@U", usuarioId);  
        cmd.Parameters.AddWithValue("@A", ambienteId);  

        int qtd = (int)cmd.ExecuteScalar();  
        return qtd > 0;  
    }  
}  

private void SalvarLog(Log log)  
{  
    using (var conn = Database.GetConnection())  
    {  
        conn.Open();  
        var cmd = new SqlCommand(  
            "INSERT INTO LogAcesso (DtAcesso, UsuarioId, AmbienteId, TipoAcesso) " +  
            "VALUES (@D, @U, @A, @T)",  
            conn);  

        cmd.Parameters.AddWithValue("@D", log.DtAcesso);  
        cmd.Parameters.AddWithValue("@U", log.UsuarioId);  
        cmd.Parameters.AddWithValue("@A", log.AmbienteId);  
        cmd.Parameters.AddWithValue("@T", log.TipoAcesso);  

        cmd.ExecuteNonQuery();  
    }  
}  

public List<Log> ListarLogs()  
{  
    var lista = new List<Log>();  

    using (var conn = Database.GetConnection())  
    {  
        conn.Open();  
        var cmd = new SqlCommand(  
            "SELECT Id, DtAcesso, UsuarioId, AmbienteId, TipoAcesso FROM LogAcesso ORDER BY DtAcesso DESC",  
            conn);  

        var reader = cmd.ExecuteReader();  
        while (reader.Read())  
        {  
            lista.Add(new Log  
            {  
                Id = reader.GetInt32(0),  
                DtAcesso = reader.GetDateTime(1),  
                UsuarioId = reader.GetInt32(2),  
                AmbienteId = reader.GetInt32(3),  
                TipoAcesso = reader.GetBoolean(4)  
            });  
        }  
    }  

    return lista;  
}  

// ======================= DOWNLOAD / UPLOAD =======================  
public static Cadastro Download()  
{  
    var cad = new Cadastro();  

    // Carrega usuários  
    using (var conn = Database.GetConnection())  
    {  
        conn.Open();  
        var cmd = new SqlCommand("SELECT Id, Nome FROM Usuario", conn);  
        var reader = cmd.ExecuteReader();  
        while (reader.Read())  
        {  
            cad.AdicionarUsuario(new Usuario(reader.GetInt32(0), reader.GetString(1)));  
        }  
    }  

    // Carrega ambientes  
    using (var conn = Database.GetConnection())  
    {  
        conn.Open();  
        var cmd = new SqlCommand("SELECT Id, Nome FROM Ambiente", conn);  
        var reader = cmd.ExecuteReader();  
        while (reader.Read())  
        {  
            cad.AdicionarAmbiente(new Ambiente(reader.GetInt32(0), reader.GetString(1)));  
        }  
    }  

    return cad;  
}  

public void Upload()  
{  
    // Salva usuários  
    using (var conn = Database.GetConnection())  
    {  
        conn.Open();  
        foreach (var u in usuarios)  
        {  
            var cmd = new SqlCommand("IF NOT EXISTS(SELECT 1 FROM Usuario WHERE Id=@Id) " +  
                                     "INSERT INTO Usuario (Id, Nome) VALUES (@Id, @Nome)", conn);  
            cmd.Parameters.AddWithValue("@Id", u.Id);  
            cmd.Parameters.AddWithValue("@Nome", u.Nome);  
            cmd.ExecuteNonQuery();  
        }  
    }  

    // Salva ambientes  
    using (var conn = Database.GetConnection())  
    {  
        conn.Open();  
        foreach (var a in ambientes)  
        {  
            var cmd = new SqlCommand("IF NOT EXISTS(SELECT 1 FROM Ambiente WHERE Id=@Id) " +  
                                     "INSERT INTO Ambiente (Id, Nome) VALUES (@Id, @Nome)", conn);  
            cmd.Parameters.AddWithValue("@Id", a.Id);  
            cmd.Parameters.AddWithValue("@Nome", a.Nome);  
            cmd.ExecuteNonQuery();  
        }  
    }  
}  


}
