using System;

namespace P3CsharpOrm.Models
{
    public class Usuario
    {
        // El ORM reconoce automáticamente "Id" como la llave primaria (PK) y serial
        public int Id { get; set; }

        public string Nombre { get; set; } = null!; // null! indica que es requerido (NOT NULL)

        public string Email { get; set; } = null!; // UNIQUE y Index se configurarán en el Fluent API

        public string? NumeroCelular { get; set; } // UNIQUE y Index se configurarán en el Fluent API

        public int? Edad { get; set; }

        public string? Direccion { get; set; }

        // Por defecto, .NET maneja DateTime. En Postgres se mapeará a timestamp
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

        public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
    }
}