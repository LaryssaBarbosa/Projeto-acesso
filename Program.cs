using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        Cadastro cadastro = new Cadastro();

        Usuario u1 = new Usuario(1, "Alice");
        Usuario u2 = new Usuario(2, "Bob");

        UsuarioDAO dao = new UsuarioDAO();
        dao.Inserir(u1);
        dao.Inserir(u2);

        Ambiente a1 = new Ambiente(1, "Sala 101");
        Ambiente a2 = new Ambiente(2, "Laboratório");

        using (var conn = Database.GetConnection())
        {
            conn.Open();
            var cmd = new SqlCommand(
                "INSERT INTO Ambiente (Id, Nome) VALUES (@Id, @Nome)", conn);

            cmd.Parameters.AddWithValue("@Id", a1.Id);
            cmd.Parameters.AddWithValue("@Nome", a1.Nome!);
            cmd.ExecuteNonQuery();

            cmd.Parameters["@Id"].Value = a2.Id;
            cmd.Parameters["@Nome"].Value = a2.Nome!;
            cmd.ExecuteNonQuery();
        }

        using (var conn = Database.GetConnection())
        {
            conn.Open();
            var cmd = new SqlCommand(
                "INSERT INTO UsuarioAmbiente (UsuarioId, AmbienteId) VALUES (@U, @A)", conn);
            cmd.Parameters.AddWithValue("@U", u1.Id);
            cmd.Parameters.AddWithValue("@A", a1.Id);
            cmd.ExecuteNonQuery();
        }

        cadastro.RegistrarAcesso(u1.Id, a1.Id);
        cadastro.RegistrarAcesso(u2.Id, a1.Id);

        var logsTeste = cadastro.ListarLogs();
        Console.WriteLine("===== LOGS TESTE RÁPIDO =====");
        foreach (var log in logsTeste)
            Console.WriteLine(log);

        Cadastro cad = Cadastro.Download();
        int opc;
        do
        {
            Console.WriteLine("\n===== MENU PRINCIPAL =====");
            Console.WriteLine("0 – Sair");
            Console.WriteLine("1 – Cadastrar ambiente");
            Console.WriteLine("2 – Consultar ambiente");
            Console.WriteLine("3 – Excluir ambiente");
            Console.WriteLine("4 – Cadastrar usuário");
            Console.WriteLine("5 – Consultar usuário");
            Console.WriteLine("6 – Excluir usuário");
            Console.WriteLine("7 – Conceder permissão");
            Console.WriteLine("8 – Revogar permissão");
            Console.WriteLine("9 – Registrar acesso");
            Console.WriteLine("10 – Consultar logs");
            Console.Write("Opção: ");

            if (!int.TryParse(Console.ReadLine(), out opc))
            {
                Console.WriteLine("Opção inválida!");
                continue;
            }

            switch (opc)
            {
                case 0:
                    cad.Upload();
                    Console.WriteLine("Encerrando...");
                    break;

                case 1:
                    Console.Write("ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int idA)) break;
                    Console.Write("Nome: ");
                    string nomeA = Console.ReadLine()!;
                    cad.AdicionarAmbiente(new Ambiente(idA, nomeA));
                    Console.WriteLine("Ambiente cadastrado!");
                    break;

                case 2:
                    Console.Write("ID do ambiente: ");
                    if (!int.TryParse(Console.ReadLine(), out int idConsA)) break;
                    var amb = cad.PesquisarAmbiente(idConsA);
                    Console.WriteLine(amb == null ? "Não encontrado!" : amb.ToString());
                    break;

                case 3:
                    Console.Write("ID do ambiente: ");
                    if (!int.TryParse(Console.ReadLine(), out int idExcA)) break;
                    var ambR = cad.PesquisarAmbiente(idExcA);
                    if (ambR == null) Console.WriteLine("Não encontrado!");
                    else { cad.RemoverAmbiente(ambR); Console.WriteLine("Ambiente removido!"); }
                    break;

                case 4:
                    Console.Write("ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int idU)) break;
                    Console.Write("Nome: ");
                    string nomeU = Console.ReadLine()!;
                    cad.AdicionarUsuario(new Usuario(idU, nomeU));
                    Console.WriteLine("Usuário cadastrado!");
                    break;

                case 5:
                    Console.Write("ID do usuário: ");
                    if (!int.TryParse(Console.ReadLine(), out int idConsU)) break;
                    var user = cad.PesquisarUsuario(idConsU);
                    Console.WriteLine(user == null ? "Não existe!" : user.ToString());
                    break;

                case 6:
                    Console.Write("ID do usuário: ");
                    if (!int.TryParse(Console.ReadLine(), out int idExcU)) break;
                    var userR = cad.PesquisarUsuario(idExcU);
                    if (userR == null) Console.WriteLine("Usuário não existe!");
                    else if (userR.Ambientes.Count > 0) Console.WriteLine("Usuário NÃO pode ser removido! Possui permissões.");
                    else { cad.RemoverUsuario(userR); Console.WriteLine("Usuário removido!"); }
                    break;

                case 7:
                    Console.Write("ID do usuário: ");
                    if (!int.TryParse(Console.ReadLine(), out int idPermU)) break;
                    var up = cad.PesquisarUsuario(idPermU);
                    Console.Write("ID do ambiente: ");
                    if (!int.TryParse(Console.ReadLine(), out int idPermA)) break;
                    var ap = cad.PesquisarAmbiente(idPermA);
                    if (up == null || ap == null) Console.WriteLine("Dados inválidos!");
                    else if (up.ConcederPermissao(ap)) Console.WriteLine("Permissão concedida!");
                    else Console.WriteLine("Já possuía permissão!");
                    break;

                case 8:
                    Console.Write("ID do usuário: ");
                    if (!int.TryParse(Console.ReadLine(), out int idRevU)) break;
                    var ur = cad.PesquisarUsuario(idRevU);
                    Console.Write("ID do ambiente: ");
                    if (!int.TryParse(Console.ReadLine(), out int idRevA)) break;
                    var ar = cad.PesquisarAmbiente(idRevA);
                    if (ur == null || ar == null) Console.WriteLine("Dados inválidos!");
                    else if (ur.RevogarPermissao(ar)) Console.WriteLine("Permissão revogada!");
                    else Console.WriteLine("O usuário não possuía essa permissão!");
                    break;

                case 9:
                    Console.Write("ID do usuário: ");
                    if (!int.TryParse(Console.ReadLine(), out int idLogU)) break;
                    var ul = cad.PesquisarUsuario(idLogU);
                    Console.Write("ID do ambiente: ");
                    if (!int.TryParse(Console.ReadLine(), out int idLogA)) break;
                    var al = cad.PesquisarAmbiente(idLogA);
                    if (ul == null || al == null) Console.WriteLine("Dados inválidos!");
                    else
                    {
                        bool autorizado = ul.Ambientes.Contains(al);
                        Log log = new Log(ul.Id, al.Id, autorizado);
                        cadastro.RegistrarAcesso(ul.Id, al.Id);
                        Console.WriteLine(autorizado ? "Acesso PERMITIDO!" : "Acesso NEGADO!");
                    }
                    break;

                case 10:
                    Console.WriteLine("===== LOGS DE ACESSO =====");
                    var todosLogs = cadastro.ListarLogs();
                    foreach (var log in todosLogs)
                        Console.WriteLine(log);
                    break;

                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }

        } while (opc != 0);
    }
}
