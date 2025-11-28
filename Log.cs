using System;
using Microsoft.Data.SqlClient;


public class Log
{
public int Id { get; set; }              
public DateTime DtAcesso { get; set; }
public int UsuarioId { get; set; }        
public int AmbienteId { get; set; }       
public bool TipoAcesso { get; set; }      

public Log() { }

public Log(int usuarioId, int ambienteId, bool tipo)
{
    DtAcesso = DateTime.Now;
    UsuarioId = usuarioId;
    AmbienteId = ambienteId;
    TipoAcesso = tipo;
}

public override string ToString()
{
    string t = TipoAcesso ? "AUTORIZADO" : "NEGADO";
    return $"{DtAcesso:dd/MM/yyyy HH:mm:ss} - Usuário {UsuarioId} - Ambiente {AmbienteId} - {t}";
}


}
