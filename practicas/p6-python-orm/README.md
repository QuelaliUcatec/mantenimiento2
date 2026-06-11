# Práctica 6: Python ORM + 3 Migraciones con Alembic (PostgreSQL)

**Proyecto:** p6-python-orm  
**Rama:** `P6-RaulHeredia`  
**Base de Datos:** PostgreSQL 16 (Contenedor activo `ucatec-postgres`)  
**ORM:** SQLAlchemy 2.0  
**Herramienta de Migración:** Alembic  

Este proyecto implementa el modelado de datos utilizando un ORM moderno en Python (SQLAlchemy) y realiza un control incremental del esquema de base de datos a través de **3 migraciones sucesivas** gestionadas por Alembic en un servidor de PostgreSQL.

---

## 1. Diagrama de Arquitectura del Sistema

El siguiente diagrama muestra el flujo de trabajo, los componentes del proyecto local en Python y cómo interactúan con el contenedor de PostgreSQL en Docker y tu editor de código:

```mermaid
graph TD
    subgraph Computadora_Host ["Entorno Local (Laptop)"]
        subgraph Entorno_Python ["Código Python"]
            Main["main.py Script"] -->|Usa la Sesión| DBConfig[database.py]
            DBConfig -->|Carga Variables| Dotenv[.env]
            Models["models/vehiculo.py"] -->|Define Entidad| DBConfig
            
            Alembic["Alembic Engine"] -->|Lee Metadatos| Models
            Alembic -->|Aplica Versiones| Versions["migrations/versions/*"]
        end
        
        subgraph Editor ["VS Code / Cursor"]
            DBExt["Extensión Database"] -.->|Conexión Visual| PGPort["Puerto Local 5432"]
        end
    end

    subgraph Contenedor_Docker ["Docker: ucatec-postgres"]
        PGServer["Postgres Server"] <--> PGPort
        
        subgraph Base_de_Datos ["Base de Datos: p6_mantenimiento"]
            TableVeh["Tabla: vehiculos"]
            TableAlembic["Tabla: alembic_version"]
        end
    end

    DBConfig -->|Consultas ORM via psycopg2| PGPort
    Alembic -->|Sentencias DDL Schema| PGPort
    PGServer <--> Base_de_Datos
```

---

## 2. Estructura del Proyecto

```text
practicas/p6-python-orm/
├── .env                  # Configuración de credenciales de PostgreSQL
├── .gitignore            # Archivos y carpetas omitidos por Git (venv, caché, etc.)
├── alembic.ini           # Configuración base de Alembic (enlaza con migrations/env.py)
├── database.py           # Configuración del Engine de SQLAlchemy y la Sesión
├── main.py               # Script de prueba automatizado de inserciones/consultas
├── requirements.txt      # Archivo de requerimientos y dependencias
├── control.sh            # Script ejecutable para gestionar migraciones por menú
├── models/
│   ├── __init__.py
│   └── vehiculo.py       # Modelo de la tabla "vehiculos" (esquema final)
└── migrations/           # Carpeta del entorno de Alembic
    ├── env.py            # Script de configuración para conectar Alembic con el ORM
    ├── README            # Documento descriptivo básico de Alembic
    ├── script.py.mako    # Plantilla de generación de migraciones
    └── versions/         # Carpeta que contiene las 3 revisiones secuenciales
```

---

## 3. Configuración y Dependencias

Sigue estas instrucciones paso a paso para inicializar el proyecto en tu entorno local:

1. **Crear y activar el entorno virtual:**
   ```bash
   python3 -m venv venv
   source venv/bin/activate
   ```

2. **Instalar dependencias necesarias:**
   ```bash
   pip install --upgrade pip
   pip install -r requirements.txt
   ```
   *Dependencias principales:*
   * `SQLAlchemy>=2.0.0`: Mapeo objeto-relacional y consultas.
   * `alembic`: Control de versiones del esquema de base de datos.
   * `psycopg2-binary`: Driver de conexión nativa de Python a PostgreSQL.
   * `python-dotenv`: Carga de variables de entorno desde el archivo `.env`.

3. **Configuración de Variables de Entorno (`.env`):**
   El archivo `.env` apunta a tu contenedor Docker activo `ucatec-postgres` con la base de datos específica `p6_mantenimiento`:
   ```env
   DATABASE_URL=postgresql+psycopg2://postgres:mariane2019@localhost:5432/p6_mantenimiento
   ```

---

## 4. Evolución del Esquema (Historial de 3 Migraciones)

El diseño de la tabla `vehiculos` se realizó de manera incremental y controlada por Alembic. A continuación se detallan las 3 revisiones consecutivas aplicadas:

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

    Migracion_1_Inicial --> Migracion_2_Placa : "Migración 1 a 2"
    Migracion_2_Placa --> Migracion_3_Detalles_Final : "Migración 2 a 3"
```

### Detalle de las Versiones:

1. **Migración 1: Creación de la Tabla Base (`f0aaca03fa75`)**
   * **Objetivo:** Inicializa la tabla `vehiculos`.
   * **Campos:**
     * `id`: Entero, Clave Primaria, Autoincremental, con Índice.
     * `marca`: Cadena (100), No nulo.
     * `modelo`: Cadena (100), No nulo.
     * `creado_en`: Fecha y hora por defecto.
   * **Código de revisión:** [f0aaca03fa75_crear_tabla_vehiculos.py](file:///home/raulito/Documentos/Gestión y Manejo de Base de Datos II/mantenimiento2/practicas/p6-python-orm/migrations/versions/f0aaca03fa75_crear_tabla_vehiculos.py)

2. **Migración 2: Adición del Campo `placa` (`1120d7d116b1`)**
   * **Objetivo:** Añade un campo para la patente/matrícula del vehículo.
   * **Campos:**
     * `placa`: Cadena (20), Única, Opcional (Nullable), con Índice.
   * **Código de revisión:** [1120d7d116b1_agregar_columna_placa.py](file:///home/raulito/Documentos/Gestión y Manejo de Base de Datos II/mantenimiento2/practicas/p6-python-orm/migrations/versions/1120d7d116b1_agregar_columna_placa.py)

3. **Migración 3: Adición de Detalles de Vehículo (`4942975705db`)**
   * **Objetivo:** Añade campos complementarios para el color y la lectura de kilometraje.
   * **Campos:**
     * `color`: Cadena (50), Opcional.
     * `kilometraje`: Entero, Opcional, con valor inicial por defecto de `0`.
   * **Código de revisión:** [4942975705db_agregar_columnas_color_y_kilometraje.py](file:///home/raulito/Documentos/Gestión y Manejo de Base de Datos II/mantenimiento2/practicas/p6-python-orm/migrations/versions/4942975705db_agregar_columnas_color_y_kilometraje.py)

---

## 5. Visualización en la Extensión "Database"

Puedes conectarte visualmente a la base de datos de tu tarea en la extensión de tu editor utilizando las siguientes credenciales:

* **Gestor:** PostgreSQL
* **Host:** `127.0.0.1` (o `localhost`)
* **Puerto:** `5432`
* **Usuario:** `postgres`
* **Contraseña:** `mariane2019`
* **Base de Datos:** `p6_mantenimiento`

### Qué observar en el panel:
* **Tabla `alembic_version`:** Si la abres, verás una fila con el valor `4942975705db`. Esto indica que la base de datos se encuentra actualizada en la última revisión de tus migraciones.
* **Tabla `vehiculos`:** Muestra las 7 columnas creadas de forma incremental. Puedes abrirla para observar los datos insertados por el script.

---

## 6. Validación con Script de Prueba (`main.py`)

Para verificar que el ORM está configurado y respondiendo correctamente a las consultas sobre el esquema PostgreSQL, ejecuta el validador:

```bash
python main.py
```

El script realiza de manera automática las siguientes acciones:
1. Abre una sesión con la base de datos PostgreSQL.
2. Limpia los registros anteriores para evitar colisiones de clave única en la columna `placa`.
3. Inserta 3 registros de tipo `Vehiculo` completando todos los campos creados en las 3 migraciones.
4. Consulta todos los registros y los imprime en la terminal confirmando el éxito de la prueba.

---

## 7. Script Interactivo de Gestión (`control.sh`)

Para facilitarte la gestión de la base de datos sin necesidad de recordar los comandos de la consola, puedes usar el menú interactivo ejecutando:

```bash
./control.sh
```

El menú te permitirá seleccionar las siguientes opciones de forma directa:
1. **Ver revisión actual en la BD**: Muestra cuál es el hash activo en PostgreSQL (`alembic current`).
2. **Ver historial de todas las migraciones**: Lista las 3 migraciones detallando el orden e ID de revisión (`alembic history --verbose`).
3. **Aplicar todas las migraciones pendientes**: Sincroniza y actualiza la base de datos (`alembic upgrade head`).
4. **Deshacer la última migración**: Revierte la última modificación del esquema en PostgreSQL (`alembic downgrade -1`).
5. **Ejecutar script de prueba**: Corre `main.py` para insertar y consultar datos.
6. **Salir**: Cierra el programa.
