using Microsoft.EntityFrameworkCore;
using P3CsharpOrm.Models;
using System;

namespace P3CsharpOrm.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor requerido para pasar la configuración (ConnectionString) al contexto
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Definimos el set de datos para Usuario (Materia lo agregaremos después)
        public DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Configuración del Modelo Usuario ---
            modelBuilder.Entity<Usuario>(entity =>
            {
                // Nombre de la tabla en PostgreSQL
                entity.ToTable("usuarios");

                // Llave Primaria
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");

                // Nombre: varchar(100), NOT NULL
                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsRequired();

                // Email: varchar(100), NOT NULL, UNIQUE, Index
                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(e => e.Email)
                    .IsUnique(); // Crea el índice único en Postgres

                // NumeroCelular: varchar(20), UNIQUE, nullable, Index
                entity.Property(e => e.NumeroCelular)
                    .HasColumnName("numero_celular")
                    .HasMaxLength(20);

                entity.HasIndex(e => e.NumeroCelular)
                    .IsUnique(); // Crea el índice único en Postgres
              
                // CreadoEn: timestamp, default: now()
                entity.Property(e => e.CreadoEn)
                    .HasColumnName("creado_en")
                    .HasDefaultValueSql("NOW()");

                // ActualizadoEn: timestamp, default: now()
                entity.Property(e => e.ActualizadoEn)
                    .HasColumnName("actualizado_en")
                    .HasDefaultValueSql("NOW()");

            });
        }
    }
}