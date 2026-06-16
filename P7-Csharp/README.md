# P7-FleetControl: Implementación de ORM con Entity Framework Core

## 1. Fundamentos Teóricos: El Ecosistema ORM

El **Object-Relational Mapping (ORM)** es una técnica de programación que permite mapear objetos de un lenguaje orientado a objetos (como C#) con las tablas de una base de datos relacional. 

Entity Framework Core (EF Core) resuelve el problema conocido como **"Impedancia Desajustada"** (*Impedance Mismatch*), es decir, la diferencia entre:
- El modelo de objetos (jerárquico, con herencia, colecciones y comportamiento).
- El modelo relacional (tabular, basado en filas y columnas).

EF Core actúa como una capa de abstracción que traduce consultas **LINQ** a sentencias SQL optimizadas. Esto trae múltiples beneficios:
- Mayor **productividad** del desarrollador.
- Seguridad contra **inyecciones SQL** (consultas parametrizadas).
- Portabilidad entre diferentes motores de bases de datos.
- Mantenimiento más sencillo del código.

## 2. Arquitectura de PostgreSQL

**PostgreSQL** es un potente sistema de gestión de bases de datos relacional (RDBMS) open-source que cumple estrictamente con las propiedades **ACID**:

- **Atomicidad**: Las transacciones se ejecutan completamente o no se ejecutan.
- **Consistencia**: Los datos siempre pasan de un estado válido a otro.
- **Aislamiento**: Las transacciones no interfieren entre sí.
- **Durabilidad**: Los cambios confirmados persisten incluso tras fallos del sistema.

Su robustez lo hace ideal para aplicaciones de gestión de flotas, donde se requieren relaciones complejas, integridad referencial y alto rendimiento.

## 3. Contenedorización con Docker

Docker permite empaquetar y ejecutar aplicaciones en contenedores aislados, eliminando el problema clásico de **"funciona en mi máquina"**.

En este proyecto:
- PostgreSQL se ejecuta en un contenedor Docker.
- Garantiza consistencia entre entornos de desarrollo, pruebas y producción.
- Facilita la portabilidad y la reproducción exacta del entorno.

## 4. Entity Framework Core y el Flujo de Migraciones

Las **migraciones** de EF Core son el mecanismo de control de versiones del esquema de base de datos. 

El flujo funciona así:
1. Se definen las entidades (clases C#) con sus propiedades y relaciones.
2. EF Core compara los modelos con el estado actual de la base de datos.
3. Genera automáticamente scripts **DDL** (Data Definition Language).
4. Permite aplicar cambios de forma incremental y reversible.

Esto asegura que la evolución del esquema sea segura, auditable y colaborativa.

## 5. Configuración de la Conexión (Connection String)

La cadena de conexión es el enlace entre la aplicación y el motor de base de datos. En este proyecto se utiliza:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=p7_csharp_orm;Username=postgres;Password=tu_contraseña"
}
```

Driver: Npgsql (proveedor oficial de PostgreSQL para .NET).
Autenticación segura mediante SASL.
Configuración flexible mediante appsettings.json y variables de entorno.

## 6. Patrones de Diseño (DbContext y Factory)

DbContext: Clase central que representa una sesión con la base de datos. Contiene los DbSet<T> y configura las relaciones.
AppDbContextFactory: Implementa el patrón Factory. Esencial para herramientas CLI (dotnet ef) ya que permite crear instancias del contexto sin depender de la inyección de dependencias en tiempo de ejecución.

Este diseño promueve la separación de responsabilidades y facilita las migraciones desde línea de comandos.
## 7. Guía Paso a Paso de Ejecución

# 1. Levantar PostgreSQL con Docker
docker run --name postgres-orm -e POSTGRES_PASSWORD=tu_contraseña -p 5432:5432 -d postgres:16

# 2. Restaurar dependencias del proyecto
dotnet restore

# 3. Aplicar migraciones
dotnet ef database update

# 4. Ejecutar la aplicación
dotnet run
## 8. Diagrama de Flujo de la Arquitectura

```mermaid
flowchart TD
    subgraph "Capa de Aplicación"
        A[Entidades C#<br/>Models] 
        B[DbContext]
        C[Repositories / Services]
    end

    subgraph "Entity Framework Core"
        D[LINQ Queries] 
        E[Query Translator<br/>+ Change Tracker]
        F[Migraciones]
        G[Npgsql Provider]
    end

    subgraph "Infraestructura"
        H[(PostgreSQL<br/>en Docker)]
    end

    A --> B
    B --> C
    C --> D
    D --> E
    E --> G
    F --> G
    G --> H

    classDef app fill:#e3f2fd,stroke:#1976d2,stroke-width:2px
    classDef ef fill:#f3e5f5,stroke:#7b1fa2,stroke-width:2px
    classDef infra fill:#e8f5e9,stroke:#388e3c,stroke-width:2px

    class A,B,C app
    class D,E,F,G ef
    class H infra
   ```
## 9. Conclusiones sobre la Persistencia de Datos
La adopción de Entity Framework Core junto con PostgreSQL y Docker representa una solución moderna, profesional y escalable para la persistencia de datos.
Beneficios clave:

Desacoplamiento claro entre lógica de negocio y almacenamiento.
Evolución segura del esquema mediante migraciones.
Entornos reproducibles gracias a contenedores.
Código limpio, mantenible y seguro.

Esta implementación es la base ideal para construir aplicaciones complejas de gestión de flotas (FleetControl), donde la integridad de los datos y la escalabilidad son críticas.
