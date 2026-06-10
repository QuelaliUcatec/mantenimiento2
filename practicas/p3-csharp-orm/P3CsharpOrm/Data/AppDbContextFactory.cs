using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace P3CsharpOrm.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
        envPath = Path.GetFullPath(envPath);
        var envFile = File.Exists(envPath) ? envPath : ".env";

        if (File.Exists(envFile))
        {
            foreach (var line in File.ReadAllLines(envFile))
            {
                var trimmed = line.Trim();
                if (trimmed.Length == 0 || trimmed.StartsWith('#')) continue;
                var sep = trimmed.IndexOf('=');
                if (sep <= 0) continue;
                var key = trimmed[..sep].Trim();
                var val = trimmed[(sep + 1)..].Trim().Trim('"', '\'');
                Environment.SetEnvironmentVariable(key, val);
            }
        }

        var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
        var db = Environment.GetEnvironmentVariable("DB_NAME") ?? "prueba_csharp";
        var user = Environment.GetEnvironmentVariable("DB_USER") ?? "dagc_user";
        var pass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "dagc_postgres_pass";

        var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={pass}";

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}
