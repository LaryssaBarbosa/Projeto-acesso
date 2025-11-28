using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;


public class UsuarioDAO
{
    public void Inserir(Usuario usuario)
    {
        using (var conn = Database.GetConnection())
        {
            conn.Open();
            string sql = "INSERT INTO Usuario (Id, Nome) VALUES (@Id, @Nome)";
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", usuario.Id);
                cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public Usuario? Consultar(int id)
    {
        using (var conn = Database.GetConnection())
        {
            conn.Open();
            string sql = "SELECT Id, Nome FROM Usuario WHERE Id = @Id";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuario(
                            reader.GetInt32(0),
                            reader.GetString(1)
                        );
                    }
                    return null;
                }
            }
        }
    }

    public bool Excluir(int id)
    {
        using (var conn = Database.GetConnection())
        {
            conn.Open();
            string sql = "DELETE FROM Usuario WHERE Id = @Id";

            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                int linhas = cmd.ExecuteNonQuery();
                return linhas > 0;
            }
        }
    }

    public List<Usuario> ListarTodos()
    {
        List<Usuario> lista = new();

        using (var conn = Database.GetConnection())
        {
            conn.Open();
            string sql = "SELECT Id, Nome FROM Usuario";

            using (var cmd = new SqlCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new Usuario(
                        reader.GetInt32(0),
                        reader.GetString(1)
                    ));
                }
            }
        }

        return lista;
    }
}
