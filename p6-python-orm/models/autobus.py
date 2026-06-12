from sqlalchemy import Column, Integer, String
from database import Base

class Autobus(Base):
    __tablename__ = "autobuses"
    
    id = Column(Integer, primary_key=True, index=True)
    marca = Column(String)
    placa = Column(String, unique=True)