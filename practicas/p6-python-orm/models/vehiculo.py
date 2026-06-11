from sqlalchemy import Column, Integer, String, DateTime
from datetime import datetime
from database import Base

class Vehiculo(Base):
    __tablename__ = "vehiculos"

    id = Column(Integer, primary_key=True, index=True)
    marca = Column(String(100), nullable=False)
    modelo = Column(String(100), nullable=False)
    placa = Column(String(20), unique=True, index=True, nullable=True)
    color = Column(String(50), nullable=True)
    kilometraje = Column(Integer, nullable=True, default=0)
    creado_en = Column(DateTime, default=datetime.utcnow)
