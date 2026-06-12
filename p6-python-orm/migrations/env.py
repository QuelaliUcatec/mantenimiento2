import os
from dotenv import load_dotenv
from logging.config import fileConfig
from sqlalchemy import engine_from_config
from sqlalchemy import pool
from alembic import context

# 1. Cargar las variables de entorno para obtener la URL de conexión
load_dotenv()

# 2. Importar la clase Base y el modelo para que Alembic los registre
from database import Base
import models.autobus 

config = context.config

# 3. Sobrescribir la URL de sqlalchemy en Alembic con nuestra variable segura
config.set_main_option("sqlalchemy.url", os.getenv("DATABASE_URL"))

if config.config_file_name is not None:
    fileConfig(config.config_file_name)

# 4. Asignar los metadatos de nuestros modelos a Alembic
target_metadata = Base.metadata

# ... (El resto del archivo permanece igual)