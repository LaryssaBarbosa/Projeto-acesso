using System;

class Program
{
    static void Main(string[] args)
    {
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

            opc = int.Parse(Console.ReadLine());

            switch (opc)
            {
                case 0:
                    cad.Upload();
                    Console.WriteLine("Encerrando...");
                    break;

                case 1:
                    Console.Write("ID: ");
                    int idA = int.Parse(Console.ReadLine());
                    Console.Write("Nome: ");
                    string nomeA = Console.ReadLine();

                    cad.AdicionarAmbiente(new Ambiente(idA, nomeA));
                    Console.WriteLine("Ambiente cadastrado!");
                    break;

                case 2:
                    Console.Write("ID do ambiente: ");
                    int idConsA = int.Parse(Console.ReadLine());
                    var amb = cad.PesquisarAmbiente(idConsA);
                    Console.WriteLine(amb == null ? "Não encontrado!" : amb.ToString());
                    break;

                case 3:
                    Console.Write("ID do ambiente: ");
                    int idExcA = int.Parse(Console.ReadLine());
                    var ambR = cad.PesquisarAmbiente(idExcA);
                    if (ambR == null) Console.WriteLine("Não encontrado!");
                    else
                    {
                        cad.RemoverAmbiente(ambR);
                        Console.WriteLine("Ambiente removido!");
                    }
                    break;

                case 4:
                    Console.Write("ID: ");
                    int idU = int.Parse(Console.ReadLine());
                    Console.Write("Nome: ");
                    string nomeU = Console.ReadLine();
                    cad.AdicionarUsuario(new Usuario(idU, nomeU));
                    Console.WriteLine("Usuário cadastrado!");
                    break;

                case 5:
                    Console.Write("ID do usuário: ");
                    int idConsU = int.Parse(Console.ReadLine());
                    var user = cad.PesquisarUsuario(idConsU);
                    Console.WriteLine(user == null ? "Não existe!" : user.ToString());
                    break;

                case 6:
                    Console.Write("ID do usuário: ");
                    int idExcU = int.Parse(Console.ReadLine());
                    var userR = cad.PesquisarUsuario(idExcU);

                    if (userR == null) Console.WriteLine("Usuário não existe!");
                    else if (userR.Ambientes.Count > 0)
                    {
                        Console.WriteLine("Usuário NÃO pode ser removido! Possui permissões.");
                    }
                    else
                    {
                        cad.RemoverUsuario(userR);
                        Console.WriteLine("Usuário removido!");
                    }
                    break;

                case 7:
                    Console.Write("ID do usuário: ");
                    int idPermU = int.Parse(Console.ReadLine());
                    var up = cad.PesquisarUsuario(idPermU);

                    Console.Write("ID do ambiente: ");
                    int idPermA = int.Parse(Console.ReadLine());
                    var ap = cad.PesquisarAmbiente(idPermA);

                    if (up == null || ap == null)
                        Console.WriteLine("Dados inválidos!");
                    else if (up.ConcederPermissao(ap))
                        Console.WriteLine("Permissão concedida!");
                    else
                        Console.WriteLine("Já possuía permissão!");
                    break;

                case 8:
                    Console.Write("ID do usuário: ");
                    int idRevU = int.Parse(Console.ReadLine());
                    var ur = cad.PesquisarUsuario(idRevU);

                    Console.Write("ID do ambiente: ");
                    int idRevA = int.Parse(Console.ReadLine());
                    var ar = cad.PesquisarAmbiente(idRevA);

                    if (ur == null || ar == null)
                        Console.WriteLine("Dados inválidos!");
                    else if (ur.RevogarPermissao(ar))
                        Console.WriteLine("Permissão revogada!");
                    else
                        Console.WriteLine("O usuário não possuía essa permissão!");
                    break;

                case 9:
                    Console.Write("ID do usuário: ");
                    int idLogU = int.Parse(Console.ReadLine());
                    var ul = cad.PesquisarUsuario(idLogU);

                    Console.Write("ID do ambiente: ");
                    int idLogA = int.Parse(Console.ReadLine());
                    var al = cad.PesquisarAmbiente(idLogA);

                    if (ul == null || al == null)
                    {
