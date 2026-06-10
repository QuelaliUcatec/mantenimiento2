using Microsoft.EntityFrameworkCore;
using P3CsharpOrm.Models;

namespace P3CsharpOrm.Data;

public class AppDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Materia> Materias { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.NumeroCelular).IsUnique();
            entity.Property(u => u.CreadoEn).HasDefaultValueSql("now()");
            entity.Property(u => u.ActualizadoEn).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Materia>(entity =>
        {
            entity.HasIndex(m => m.Nombre);
            entity.Property(m => m.CreadoEn).HasDefaultValueSql("now()");
            entity.Property(m => m.ActualizadoEn).HasDefaultValueSql("now()");
        });
    }
}
