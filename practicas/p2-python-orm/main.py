from database import engine, Base
from models.user import User

# Crear todas las tablas definidas en los modelos
Base.metadata.create_all(bind=engine)

print("✅ Tablas creadas exitosamente en PostgreSQL")