import os
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker, declarative_base
from dotenv import load_dotenv

# Esto busca el archivo .env en el directorio actual (donde se ejecuta el comando)
load_dotenv() 

SQLALCHEMY_DATABASE_URL = os.getenv("DATABASE_URL")

# Debugging: imprime esto para ver si realmente detecta algo
print(f"DEBUG: URL leída es: {SQLALCHEMY_DATABASE_URL}")

if not SQLALCHEMY_DATABASE_URL:
    raise ValueError("ERROR: No se encontró la variable DATABASE_URL. Revisa el archivo .env.")

engine = create_engine(SQLALCHEMY_DATABASE_URL)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
Base = declarative_base()