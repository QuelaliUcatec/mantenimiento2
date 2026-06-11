from database import SessionLocal
from models.vehiculo import Vehiculo

def test_db():
    print("=== Conectando a la base de datos y creando sesión ===")
    db = SessionLocal()
    try:
        # Limpiar la tabla para evitar duplicaciones en ejecuciones repetidas
        db.query(Vehiculo).delete()
        db.commit()

        # Insertar registros de prueba usando los campos de las 3 migraciones
        print("\n=== Insertando vehículos de prueba (con campos de las 3 migraciones) ===")
        v1 = Vehiculo(marca="Toyota", modelo="Hilux", placa="1234-ABC", color="Rojo", kilometraje=15000)
        v2 = Vehiculo(marca="Suzuki", modelo="Swift", placa="5678-DEF", color="Azul", kilometraje=5000)
        v3 = Vehiculo(marca="Nissan", modelo="Sentra", placa="9012-GHI", color="Blanco", kilometraje=45000)
        
        db.add_all([v1, v2, v3])
        db.commit()
        print("¡Registros guardados con éxito!")

        # Consultar y mostrar los registros
        print("\n=== Consultando vehículos guardados desde la base de datos ===")
        vehiculos = db.query(Vehiculo).all()
        for v in vehiculos:
            print(f"ID: {v.id} | Marca: {v.marca} | Modelo: {v.modelo} | Placa: {v.placa} | Color: {v.color} | Kilometraje: {v.kilometraje} km | Creado: {v.creado_en}")
            
    except Exception as e:
        db.rollback()
        print(f"Error durante la prueba: {e}")
    finally:
        db.close()

if __name__ == "__main__":
    test_db()
