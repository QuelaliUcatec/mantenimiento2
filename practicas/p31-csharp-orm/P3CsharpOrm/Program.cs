using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using P3CsharpOrm.Data;

namespace P3CsharpOrm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🚀 Iniciando conector ORM - p3-csharp-orm...");

            // 1. Cargar el archivo .env manualmente (buscando desde la raíz si es necesario)
            string basePath = AppContext.BaseDirectory;
            string envPath = Path.Combine(basePath, ".env");

            while (!File.Exists(envPath) && Directory.GetParent(basePath) != null)
            {
                basePath = Directory.GetParent(basePath)!.FullName;
                envPath = Path.Combine(basePath, ".env");
            }

            if (File.Exists(envPath))
            {
                DotNetEnv.Env.Load(envPath);
                Console.WriteLine("✅ Archivo .env cargado correctamente.");
            }
            else
            {
                Console.WriteLine("⚠️ Advertencia: No se encontró el archivo .env.");
            }

            // 2. Extraer variables de entorno
            string host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            string port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
            string user = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
            string password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";
            string database = Environment.GetEnvironmentVariable("DB_NAME") ?? "prueba_csharp";

            string connectionString = $"Host={host};Port={port};Username={user};Password={password};Database={database};";

            // 3. Inicializar el DbContext para la aplicación
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            using (var context = new AppDbContext(optionsBuilder.Options))
            {
                Console.WriteLine($"🔍 Verificando conexión a la base de datos '{database}' en {host}:{port}...");

                // 4. Verificar si la base de datos es accesible
                if (context.Database.CanConnect())
                {
                    Console.WriteLine("🎉 ¡Conexión exitosa! El ORM se comunica perfectamente con PostgreSQL.");
                }
                else
                {
                    Console.WriteLine("❌ No se pudo conectar a la base de datos.");
                    Console.WriteLine("💡 Nota: Asegúrate de que el contenedor de Docker esté corriendo y de haber ejecutado las migraciones primero.");
                }
            }

            Console.WriteLine("🏁 Finalizando ejecución del conector.");
        }
    }
}