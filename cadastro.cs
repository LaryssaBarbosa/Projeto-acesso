using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Cadastro
{
    public List<Usuario> Usuarios { get; set; } = new();
    public List<Ambiente> Ambientes { get; set; } = new();

    private static string pasta = "Dados";
    private static string arqUsuarios = Path.Combine(pasta, "usuarios.json");
    private static string arqAmbientes = Path.Combine(pasta, "ambientes.json");

    public void AdicionarUsuario(Usuario u) => Usuarios.Add(u);
    public bool RemoverUsuario(Usuario u) => Usuarios.Remove(u);

    public Usuario PesquisarUsuario(int id)
        => Usuarios.Find(u => u.Id == id);

    public void AdicionarAmbiente(Ambiente a) => Ambientes.Add(a);
    public bool RemoverAmbiente(Ambiente a) => Ambientes.Remove(a);

    public Ambiente PesquisarAmbiente(int id)
        => Ambientes.Find(a => a.Id == id);

    public void RegistrarAcesso(Usuario u, Ambiente a)
    {
        bool autorizado = u.Ambientes.Contains(a);
        a.RegistrarLog(new Log(u, autorizado));
    }

    public void Upload()
    {
        if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

        File.WriteAllText(arqUsuarios, JsonSerializer.Serialize(Usuarios));
        File.WriteAllText(arqAmbientes, JsonSerializer.Serialize(Ambientes));
    }

    public static Cadastro Download()
    {
        Cadastro cad = new();

        if (File.Exists(arqUsuarios))
            cad.Usuarios = JsonSerializer.Deserialize<List<Usuario>>(File.ReadAllText(arqUsuarios));

        if (File.Exists(arqAmbientes))
            cad.Ambientes = JsonSerializer.Deserialize<List<Ambiente>>(File.ReadAllText(arqAmbientes));

        return cad;
    }
}
