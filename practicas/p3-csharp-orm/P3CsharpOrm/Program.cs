using Microsoft.EntityFrameworkCore;
using P3CsharpOrm.Data;

var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
envPath = Path.GetFullPath(envPath);
if (File.Exists(envPath))
{
    foreach (var line in File.ReadAllLines(envPath))
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

using var context = new AppDbContext(optionsBuilder.Options);

if (context.Database.CanConnect())
    Console.WriteLine("Conectado a PostgreSQL correctamente.");
else
    Console.WriteLine("Error: no se pudo conectar a PostgreSQL.");
