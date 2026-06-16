using Microsoft.EntityFrameworkCore;
using P7_FleetControl.Models;
using System;

namespace P7_FleetControl.Data;

public class AppDbContext : DbContext
{
    public DbSet<Autobus> Autobuses { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Usamos la cadena directa para asegurar que funcione sin depender de variables externas
            string connString = "Host=localhost;Port=5432;Database=p7_csharp_orm;Username=postgres;Password=root123;";
            optionsBuilder.UseNpgsql(connString);
        }
    }
}