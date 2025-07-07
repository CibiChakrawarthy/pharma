using System.Data;
using Api.Routes;
using Microsoft.Data.Sqlite;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        const string dbFile = "grossprofit.db";
        const string connStr = $"Data Source={dbFile}";
        if (File.Exists(dbFile))
            File.Delete(dbFile);

        using (SqliteConnection conn = new(connStr))
        {
            conn.Open();
            string sqlPath = Path.Combine(AppContext.BaseDirectory, "seed.sql");
            string sql = File.ReadAllText(sqlPath);
            using SqliteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        builder.Services.AddTransient<IDbConnection>(_ => new SqliteConnection(connStr));

        WebApplication app = builder.Build();

        app.MapGrossProfitEndpoints();

        app.Run();
    }
}
