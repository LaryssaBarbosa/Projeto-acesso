using System.Collections.Generic;
using Microsoft.Data.SqlClient;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty; // Inicializado para evitar warnings de nulidade
    public List<Ambiente> Ambientes { get; set; } = new(); // Inicialização segura

    public Usuario() { } // Construtor sem parâmetros, útil para deserialização ou ORM

    public Usuario(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }

    public bool ConcederPermissao(Ambiente ambiente)
    {
        if (Ambientes.Contains(ambiente)) return false;
        Ambientes.Add(ambiente);
        return true;
    }

    public bool RevogarPermissao(Ambiente ambiente)
    {
        return Ambientes.Remove(ambiente);
    }

    public override string ToString()
    {
        return $"[ID={Id}] {Nome}";
    }
}
