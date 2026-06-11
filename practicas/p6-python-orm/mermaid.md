# Diagramas de Arquitectura y Evolución del Esquema (Mermaid)

Este archivo contiene los diagramas descriptivos de la **Práctica 6** representados mediante Mermaid.

---

## 1. Evolución Incremental de la Tabla `vehiculos` (Las 3 Migraciones)

El siguiente diagrama de clases muestra cómo se agregaron campos progresivamente en cada migración de Alembic:

```mermaid
classDiagram
    direction LR
    class Migracion_1_Inicial {
        +Integer id (PK)
        +String(100) marca
        +String(100) modelo
        +DateTime creado_en
    }
    
    class Migracion_2_Placa {
        +Integer id (PK)
        +String(100) marca
        +String(100) modelo
        +String(20) placa (UQ, IX)
        +DateTime creado_en
    }
    
    class Migracion_3_Detalles_Final {
        +Integer id (PK)
        +String(100) marca
        +String(100) modelo
        +String(20) placa (UQ, IX)
        +String(50) color
        +Integer kilometraje
        +DateTime creado_en
    }

    Migracion_1_Inicial --> Migracion_2_Placa : "Migración 2 (1120d7d116b1) <br> Añade campo 'placa' (único)"
    Migracion_2_Placa --> Migracion_3_Detalles_Final : "Migración 3 (4942975705db) <br> Añade campos 'color' y 'kilometraje'"
```

---

## 2. Diagrama de Arquitectura del Sistema

Este diagrama ilustra cómo interactúa el script de Python, Alembic, el entorno virtual, el contenedor de Docker con PostgreSQL y la extensión "Database" de tu editor:

```mermaid
graph TD
    subgraph Computadora_Host [Entorno Local (Laptop)]
        subgraph Entorno_Python [Código Python]
            Main[main.py Script] -->|Usa la Sesión| DBConfig[database.py]
            DBConfig -->|Carga Variables| Dotenv[.env]
            Models[models/vehiculo.py] -->|Define Entidad| DBConfig
            
            Alembic[Alembic Engine] -->|Lee Metadatos| Models
            Alembic -->|Aplica Versiones| Versions[migrations/versions/*]
        end
        
        subgraph Editor [VS Code / Cursor]
            DBExt[Extensión Database] -.->|Conexión Visual| PGPort[Puerto Local 5432]
        end
    end

    subgraph Contenedor_Docker [Docker: ucatec-postgres]
        PGServer[Postgres Server] <--> PGPort
        
        subgraph Base_de_Datos [Base de Datos: p6_mantenimiento]
            TableVeh[Tabla: vehiculos]
            TableAlembic[Tabla: alembic_version]
        end
    end

    DBConfig -->|Consultas ORM via psycopg2| PGPort
    Alembic -->|Sentencias DDL Schema| PGPort
    PGServer <--> Base_de_Datos
```

---

## 3. Flujo de Trabajo para el Control de Migraciones

Flujo lógico al ejecutar comandos o el script `./control.sh`:

```mermaid
flowchart TD
    Start([Inicio]) --> Menu{¿Qué deseas hacer?}
    
    Menu -->|1. Ver estado| Current[Ejecuta: alembic current]
    Menu -->|2. Ver historial| History[Ejecuta: alembic history --verbose]
    Menu -->|3. Aplicar cambios| Upgrade[Ejecuta: alembic upgrade head]
    Menu -->|4. Revertir cambio| Downgrade[Ejecuta: alembic downgrade -1]
    Menu -->|5. Probar BD| TestRun[Ejecuta: python main.py]
    
    Current --> End([Fin])
    History --> End
    Upgrade -->|Modifica esquema en Postgres| End
    Downgrade -->|Deshace columnas en Postgres| End
    TestRun -->|Inserta y lee registros| End
```
