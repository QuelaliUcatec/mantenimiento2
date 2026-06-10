using Microsoft.EntityFrameworkCore;
using P3CsharpOrm.Data;
using P3CsharpOrm.Models;

string GetEnv(string key)
{
    var value = Environment.GetEnvironmentVariable(key);
    if (string.IsNullOrEmpty(value))
        throw new InvalidOperationException($"Variable de entorno '{key}' no definida.");
    return value;
}

void LoadEnv(string path)
{
    if (!File.Exists(path)) return;
    foreach (var line in File.ReadAllLines(path))
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

var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
envPath = Path.GetFullPath(envPath);
if (File.Exists(envPath))
{
    LoadEnv(envPath);
    Console.WriteLine($".env cargado desde: {envPath}");
}
else
{
    var localEnv = Path.Combine(Directory.GetCurrentDirectory(), ".env");
    if (File.Exists(localEnv))
        LoadEnv(localEnv);
}

var host = GetEnv("DB_HOST");
var port = GetEnv("DB_PORT");
var db = GetEnv("DB_NAME");
var user = GetEnv("DB_USER");
var pass = GetEnv("DB_PASSWORD");

var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={pass}";

Console.WriteLine($"Conectando a: {host}:{port}/{db}");

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseNpgsql(connectionString);

using var context = new AppDbContext(optionsBuilder.Options);

Console.WriteLine("Base de datos lista. (Las migraciones se aplican con `dotnet ef database update`)");

if (!context.Usuarios.Any())
{
    var usuario = new Usuario
    {
        Nombre = "Gastón Quelali",
        Email = "gaston@ucatec.edu",
        Edad = 30,
        NumeroCelular = "123456789"
    };
    context.Usuarios.Add(usuario);

    var materia = new Materia
    {
        Nombre = "Mantenimiento de Software 2",
        Descripcion = "Práctica de ORM con C# y Entity Framework Core",
        ContenidoMinimo = "ORM, EF Core, Npgsql, PostgreSQL"
    };
    context.Materias.Add(materia);

    context.SaveChanges();
    Console.WriteLine("Datos de ejemplo insertados.");
}
else
{
    Console.WriteLine("La base de datos ya contiene datos.");
}

Console.WriteLine("\nUsuarios:");
foreach (var u in context.Usuarios)
{
    Console.WriteLine($"  - {u.Id}: {u.Nombre} ({u.Email})");
}

Console.WriteLine("\nMaterias:");
foreach (var m in context.Materias)
{
    Console.WriteLine($"  - {m.Id}: {m.Nombre}");
}
