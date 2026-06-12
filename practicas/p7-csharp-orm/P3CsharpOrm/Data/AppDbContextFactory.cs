using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;

namespace P3CsharpOrm.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // 1. Buscar el archivo .env subiendo niveles desde la carpeta de ejecución bin/Debug/...
            // Esto es necesario porque al hacer migraciones, el CLI corre desde una ruta profunda.
            string basePath = AppContext.BaseDirectory;
            string envPath = Path.Combine(basePath, ".env");

            // Subimos carpetas hasta encontrar el .env en la raíz del proyecto si no está en el bin
            while (!File.Exists(envPath) && Directory.GetParent(basePath) != null)
            {
                basePath = Directory.GetParent(basePath)!.FullName;
                envPath = Path.Combine(basePath, ".env");
            }

            // 2. Cargar las variables de entorno del archivo .env
            if (File.Exists(envPath))
            {
                DotNetEnv.Env.Load(envPath);
            }
            else
            {
                // Si llegamos aquí y no existe, asumimos que las variables se inyectarán por sistema (ej. Docker)
                Console.WriteLine("⚠️ Advertencia: No se encontró el archivo .env. Usando variables de entorno del sistema.");
            }

            // 3. Leer las variables necesarias
            string host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            string port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
            string user = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
            string password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";
            string database = Environment.GetEnvironmentVariable("DB_NAME") ?? "prueba_csharp";

            // 4. Construir la cadena de conexión (Connection String) para PostgreSQL / Npgsql
            string connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={database};";

            // 5. Configurar las opciones del DbContext indicando que use Npgsql
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            // 6. Retornar una nueva instancia del contexto lista para que la use el CLI de EF Core
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}