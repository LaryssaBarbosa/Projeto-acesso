using Microsoft.Data.SqlClient;

public static class Database
{
private static string connectionString =
@"Server=DESKTOP-377OTC5\SQLEXPRESS;Database=ProjetoAcessoo;Trusted_Connection=True;TrustServerCertificate=True;";

public static SqlConnection GetConnection()
{
    return new SqlConnection(connectionString);
}

}
