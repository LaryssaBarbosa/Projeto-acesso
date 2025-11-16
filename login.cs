using System;

public class Log
{
    public DateTime DtAcesso { get; set; }
    public Usuario Usuario { get; set; }
    public bool TipoAcesso { get; set; } // true = autorizado

    public Log(Usuario usuario, bool tipo)
    {
        DtAcesso = DateTime.Now;
        Usuario = usuario;
        TipoAcesso = tipo;
    }

    public override string ToString()
    {
        string t = TipoAcesso ? "AUTORIZADO" : "NEGADO";
        return $"{DtAcesso:dd/MM/yyyy HH:mm:ss} - {Usuario.Nome} - {t}";
    }
}
