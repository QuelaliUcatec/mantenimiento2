# Diagramas de Arquitectura y Evolución del Esquema (C# EF Core)

Este archivo contiene los diagramas descriptivos de la **Práctica 7** representados en formato Mermaid.

---

## 1. Evolución Incremental de la Tabla `usuarios` (Las 3 Migraciones)

El siguiente diagrama de clases muestra cómo se agregaron campos progresivamente en cada migración de Entity Framework Core:

```mermaid
classDiagram
    direction LR
    class Migracion_1_CreacionTablaUsuario {
        +Integer Id
        +String Nombre
        +String Email
        +String NumeroCelular
        +DateTime CreadoEn
        +DateTime ActualizadoEn
    }
    
    class Migracion_2_AgregarEdadUsuario {
        +Integer Id
        +String Nombre
        +String Email
        +String NumeroCelular
        +Integer Edad
        +DateTime CreadoEn
        +DateTime ActualizadoEn
    }
    
    class Migracion_3_AgregarDireccionUsuario {
        +Integer Id
        +String Nombre
        +String Email
        +String NumeroCelular
        +Integer Edad
        +String Direccion
        +DateTime CreadoEn
        +DateTime ActualizadoEn
    }

    Migracion_1_CreacionTablaUsuario --> Migracion_2_AgregarEdadUsuario : Migracion_2_Agregar_edad
    Migracion_2_AgregarEdadUsuario --> Migracion_3_AgregarDireccionUsuario : Migracion_3_Agregar_direccion
```

---

## 2. Diagrama de Arquitectura del Sistema

Este diagrama ilustra cómo interactúa el código de C# (.NET 10), el CLI de Entity Framework Core, el contenedor de Docker con PostgreSQL y la extensión "Database" de tu editor:

```mermaid
graph TD
    subgraph Computadora_Host ["Entorno Local (Laptop)"]
        subgraph Entorno_Dotnet ["Código .NET 10 / C#"]
            Program["Program.cs Entrypoint"] -->|Usa Contexto| AppDbContextNode["Data/AppDbContext.cs"]
            AppDbContextNode -->|Carga variables| DotEnvFile["Archivo .env"]
            ModelsNode["Models/Usuario.cs"] -->|Mapea Entidad| AppDbContextNode
            
            EFCLI["EF Core CLI Tool (dotnet ef)"] -->|Lee Factory| DbFactoryNode["Data/AppDbContextFactory.cs"]
            EFCLI -->|Genera / Aplica| MigrationsFolder["Carpeta Migrations"]
        end
        
        subgraph Editor ["VS Code / Cursor"]
            DBExt["Extensión Database"] -.->|Conexión Visual| PGPortNode["Puerto Local 5432"]
        end
    end

    subgraph Contenedor_Docker ["Docker: ucatec-postgres"]
        PGServerNode["Servidor Postgres"] <--> PGPortNode
        
        subgraph Base_de_Datos ["Base de Datos: p7_csharp_orm"]
            TableUsuariosNode["Tabla usuarios"]
            TableEFHistoryNode["Tabla __EFMigrationsHistory"]
        end
    end

    AppDbContextNode -->|Consultas SQL via Npgsql| PGPortNode
    EFCLI -->|Sentencias DDL Schema| PGPortNode
    PGServerNode <--> Base_de_Datos
```

---

## 3. Flujo del Panel de Control (`control.sh`)

Flujo lógico de opciones dentro del menú interactivo en Bash:

```mermaid
flowchart TD
    Start([Inicio]) --> Menu{¿Qué opción deseas ejecutar?}
    
    Menu -->|1. Aplicar Migraciones| Update["Ejecuta: dotnet ef database update"]
    Menu -->|2. Ejecutar Aplicación| Run["Ejecuta: dotnet run"]
    Menu -->|3. Revertir Migración| Downgrade["Ejecuta: dotnet ef database update [nombre]"]
    Menu -->|4. Salir| Exit([Fin])
    
    Update -->|Actualiza base de datos en Postgres| FinNode([Fin de tarea])
    Run -->|Valida conexión al contenedor| FinNode
    Downgrade -->|Deshace columnas en Postgres| FinNode
```
