from sqlalchemy import Column, Integer, String, Text, DateTime
from datetime import datetime
from database import Base


class Usuario(Base):
    __tablename__ = "usuarios"

    id = Column(Integer, primary_key=True, index=True)
    nombre = Column(String(100), nullable=False)
    email = Column(String(100), unique=True, nullable=False, index=True)
    edad = Column(Integer, nullable=True)
    creado_en = Column(DateTime, default=datetime.utcnow)
    contraseña_hash = Column(String(255), nullable=True)
    actualizado_en = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    numero_celular = Column(String(20), nullable=True, unique=True, index=True)

class Materia(Base):
    __tablename__ = "materias"

    id = Column(Integer, primary_key=True, index=True)
    
    nombre = Column(String(150), nullable=False, index=True)
    descripcion = Column(Text, nullable=True)
    contenido_minimo = Column(Text, nullable=True)
    
    creado_en = Column(DateTime, default=datetime.utcnow)
    actualizado_en = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)