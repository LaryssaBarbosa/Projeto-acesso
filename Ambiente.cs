using System.Collections.Generic;
using Microsoft.Data.SqlClient;

public class Ambiente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty; // Inicializado para evitar warnings de nulidade

    public Ambiente() { }

    public Ambiente(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }

    public override string ToString()
    {
        return $"[ID={Id}] {Nome}";
    }
}
