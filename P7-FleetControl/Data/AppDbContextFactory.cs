using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;   

namespace P7_FleetControl.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        DotNetEnv.Env.Load();
        
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        string connString = $"Host={Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost"};" +
                    $"Port={Environment.GetEnvironmentVariable("DB_PORT") ?? "5432"};" +
                    $"Database={Environment.GetEnvironmentVariable("DB_NAME") ?? "p7_csharp_orm"};" +
                    $"Username={Environment.GetEnvironmentVariable("DB_USER") ?? "postgres"};" +
                    $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "root123"};";
                    
        optionsBuilder.UseNpgsql(connString);

        return new AppDbContext(optionsBuilder.Options);
    }
}