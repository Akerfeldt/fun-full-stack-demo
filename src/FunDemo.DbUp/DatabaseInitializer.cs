using Dapper;
using Microsoft.Data.SqlClient;

namespace FunDemo.DbUp;

public class DatabaseInitializer
{
    private readonly string _masterConnectionString;
    private readonly string _userConnectionString;
    private readonly string _userDatabaseName;

    public DatabaseInitializer(string masterConnectionString, string userConnectionString)
    {
        _masterConnectionString = masterConnectionString ?? throw new ArgumentNullException(nameof(masterConnectionString));
        _userConnectionString = userConnectionString ?? throw new ArgumentNullException(nameof(userConnectionString));
        _userDatabaseName = new SqlConnectionStringBuilder(_userConnectionString).InitialCatalog;
    }

    public void InitializeDatabase()
    {
        EnsureDatabaseExists();
        CreateSchema();
    }

    private void CreateSchema()
    {
        var sql = Read("FunDemo.DbUp.Resources.DbUp.sql");
        RunScript(sql);
    }

    private static string Read(string resourceName)
    {
        using var stream = typeof(DatabaseInitializer).Assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream!);
        return reader.ReadToEnd();
    }

    private void RunScript(string sql)
    {
        using var conn = new SqlConnection(_userConnectionString);
        foreach (var batch in sql.Split(["GO\r\n"], StringSplitOptions.RemoveEmptyEntries))
        {
            if (string.IsNullOrWhiteSpace(batch))
                continue;

            conn.Execute(batch, commandTimeout: 600);
        }
    }

    private void EnsureDatabaseExists()
    {
        using var conn = new SqlConnection(_masterConnectionString);
        conn.Execute($"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{_userDatabaseName}') BEGIN CREATE DATABASE [{_userDatabaseName}]; END;");
    }
}