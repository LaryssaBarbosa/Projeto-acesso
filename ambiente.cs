using System.Collections.Generic;

public class Ambiente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public Queue<Log> Logs { get; set; } = new Queue<Log>();

    public Ambiente(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }

    public void RegistrarLog(Log log)
    {
        if (Logs.Count == 100)
            Logs.Dequeue();

        Logs.Enqueue(log);
    }

    public override string ToString()
    {
        return $"[ID={Id}] {Nome}";
    }
}
