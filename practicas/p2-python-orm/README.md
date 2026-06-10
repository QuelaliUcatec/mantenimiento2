# Configuración de Alembic + SQLAlchemy

**Proyecto:** p2-python-orm  
**Fecha:** 03 de junio de 2026

## Historia de Comandos que Funcionaron

```bash
docker exec -it dagc_postgres psql -U dagc_user -d dagc_platform -h 127.0.0.1 -W
rm -rf migrations/versions/*
alembic stamp base
alembic revision --autogenerate -m "crear tabla users"
alembic upgrade head
```

---

## Estructura del Proyecto

```
p2-python-orm/
├── database.py
├── models/
│   └── user.py
├── migrations/
│   └── versions/
├── .env
├── alembic.ini
├── main.py
└── ALEMBIC_SETUP.md
```

## 1. Archivo `.env`

```env
POSTGRES_HOST=localhost
POSTGRES_PORT=5432
POSTGRES_DB=dagc_platform
POSTGRES_USER=dagc_user
POSTGRES_PASSWORD=dagc_postgres_pass
```

## 2. Archivo `database.py`

```python
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker, DeclarativeBase
from dotenv import load_dotenv
import os

load_dotenv()

DATABASE_URL = f"postgresql+psycopg2://{os.getenv('POSTGRES_USER')}:{os.getenv('POSTGRES_PASSWORD')}@{os.getenv('POSTGRES_HOST')}:{os.getenv('POSTGRES_PORT')}/{os.getenv('POSTGRES_DB')}"

engine = create_engine(DATABASE_URL, echo=True)

SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

class Base(DeclarativeBase):
    pass
```

## 3. Configuración de Alembic (`migrations/env.py`)

(Usar la versión robusta que configuramos anteriormente con `sys.path.insert`)

## 4. Flujo de Trabajo Recomendado

### Crear migración:
```bash
alembic revision --autogenerate -m "descripcion clara del cambio"
```

### Aplicar migraciones:
```bash
alembic upgrade head
```

### Comandos útiles:
- `alembic history` → Ver historial
- `alembic current` → Ver revisión actual
- `alembic downgrade -1` → Deshacer última migración

## 5. Notas Importantes

- Este proyecto usa **PostgreSQL en Docker**.
- Siempre resetear Alembic con `rm -rf migrations/versions/*` + `alembic stamp base` cuando haya problemas de revisiones.
- No usar `Base.metadata.create_all()` una vez que Alembic esté en producción.

---

**Documento creado para referencia rápida.**


# Instalación del archivo de requerimientos

```
# 1. Clonar el repositorio (si aún no lo hiciste)
git clone <url-del-repositorio>
cd p2-python-orm

# 2. Crear el entorno virtual
python -m venv venv

# 3. Activar el entorno virtual
# En macOS / Linux:
source venv/bin/activate

# En Windows:
# venv\Scripts\activate

# 4. Actualizar pip (recomendado)
pip install --upgrade pip

# 5. Instalar todas las dependencias
pip install -r requirements.txt
```
