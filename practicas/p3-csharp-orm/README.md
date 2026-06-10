# Práctica 3: ORM con C# y PostgreSQL — Entity Framework Core

**Proyecto:** p3-csharp-orm  
**Base de datos:** `prueba_csharp`  
**Fecha:** Junio 2026  
**Stack:** .NET 9 Console App + Entity Framework Core 9.0 + Npgsql 9.0 (PostgreSQL)

---

## Objetivo

Implementar un mapeo objeto-relacional (ORM) en C# utilizando **Entity Framework Core** con **PostgreSQL**, replicando los mismos modelos de la práctica anterior (`Usuario` y `Materia`) para comparar el enfoque entre Python/SQLAlchemy y C#/EF Core.

---

## Requisitos

- [.NET SDK 9.0+](https://dotnet.microsoft.com/download)
- PostgreSQL corriendo (usar el mismo contenedor de `docker-compose.yml` en `p1-mysql/`)
- Herramienta `dotnet-ef` instalada globalmente:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

---

## Estructura del Proyecto

```
p3-csharp-orm/
├── P3CsharpOrm/
│   ├── Program.cs                  # Punto de entrada de la consola
│   ├── Data/
│   │   ├── AppDbContext.cs         # DbContext con Fluent API
│   │   └── AppDbContextFactory.cs  # Fábrica para migraciones en tiempo de diseño
│   ├── Models/
│   │   ├── Usuario.cs              # Modelo Usuario
│   │   └── Materia.cs              # Modelo Materia
│   ├── Migrations/                 # Migraciones generadas por dotnet-ef
│   └── P3CsharpOrm.csproj          # Archivo de proyecto
├── .env                            # Variables de entorno (DB_NAME=prueba_csharp)
├── .gitignore
└── README.md
```

---

## Implementación

### Modelos

**`Models/Usuario.cs`**

| Campo             | Tipo C#       | Tipo PostgreSQL | Restricciones          |
|-------------------|---------------|-----------------|------------------------|
| Id                | int           | serial          | PK, autogenerado       |
| Nombre            | string        | varchar(100)    | NOT NULL               |
| Email             | string        | varchar(100)    | UNIQUE, NOT NULL, Index|
| Edad              | int?          | integer         | nullable               |
| CreadoEn          | DateTime      | timestamp       | default: now()         |
| ContraseñaHash    | string?       | varchar(255)    | nullable               |
| ActualizadoEn     | DateTime      | timestamp       | default: now()         |
| NumeroCelular     | string?       | varchar(20)     | UNIQUE, nullable, Index|

**`Models/Materia.cs`**

| Campo           | Tipo C#       | Tipo PostgreSQL | Restricciones          |
|-----------------|---------------|-----------------|------------------------|
| Id              | int           | serial          | PK, autogenerado       |
| Nombre          | string        | varchar(150)    | NOT NULL, Index        |
| Descripcion     | string?       | text            | nullable               |
| ContenidoMinimo | string?       | text            | nullable               |
| CreadoEn        | DateTime      | timestamp       | default: now()         |
| ActualizadoEn   | DateTime      | timestamp       | default: now()         |

### DbContext (`Data/AppDbContext.cs`)

- Hereda de `DbContext` con `DbSet<Usuario>` y `DbSet<Materia>`
- Configura índices únicos (Email, NumeroCelular) y valores por defecto vía **Fluent API** en `OnModelCreating`

### Fábrica de diseño (`Data/AppDbContextFactory.cs`)

Implementa `IDesignTimeDbContextFactory<AppDbContext>` para que el CLI de EF Core pueda crear el contexto en tiempo de diseño sin ejecutar la aplicación. Lee las variables desde `.env`.

### Program.cs

- Carga `.env` manualmente
- Construye la connection string para Npgsql
- Verifica la conexión con `context.Database.CanConnect()`
- No tiene lógica de negocio, seed data ni UI — es solo un conector ORM-BD

---

## Flujo de trabajo

```bash
# 1. Navegar al proyecto
cd practicas/p3-csharp-orm/P3CsharpOrm

# 2. Crear una migración cuando cambies los modelos
dotnet ef migrations add "Descripcion del cambio"

# 3. Aplicar migraciones a la base de datos
dotnet ef database update

# 4. Verificar conexión
dotnet run
```

## Comandos Útiles

```bash
# Crear una migración
dotnet ef migrations add "Descripcion del cambio"

# Aplicar migraciones
dotnet ef database update

# Revertir última migración
dotnet ef migrations remove

# Ver migraciones pendientes
dotnet ef migrations list
```

---

## Comparativa Python vs C#

| Concepto               | Python (SQLAlchemy)               | C# (EF Core)                         |
|------------------------|-----------------------------------|--------------------------------------|
| ORM                    | SQLAlchemy 2.0                    | Entity Framework Core 9.0            |
| Migraciones            | Alembic                           | dotnet-ef migrations                 |
| Configuración          | .env + database.py                | .env + AppDbContextFactory            |
| DbContext / Base       | DeclarativeBase                   | DbContext                            |
| Modelos                | Clases con Column()               | POCO classes + Data Annotations      |
| Fluent API             | (no aplica)                       | OnModelCreating                      |
| Cadena de conexión     | postgresql+psycopg2://...         | Host=...;Database=...;Username=...   |
| Seeds                  | (manual)                          | (no aplica)                          |

---

## Notas

- La base de datos `prueba_csharp` se crea automáticamente al ejecutar `dotnet ef database update`.
- Usar el mismo servidor PostgreSQL del `docker-compose.yml` (puerto `5432`).
- Es una base distinta a `dagc_platform` (usada en Python), no hay conflicto.
- Las migraciones se aplican **manualmente** con `dotnet ef database update`, no desde el código de la app.  
  Esto separa la responsabilidad de migrar (una vez al desplegar) de la de ejecutar la aplicación (cada vez que corre).
- Las migraciones de C# generan una tabla `__EFMigrationsHistory` (equivalente a `alembic_version`).
- **Target framework:** .NET 9.0 — **EF Core:** 9.0.0 — **Npgsql:** 9.0.0
