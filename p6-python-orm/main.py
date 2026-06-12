from database import SessionLocal
from models.autobus import Autobus

def registrar_y_consultar():
    # Iniciamos la sesión con la base de datos
    db = SessionLocal()

    try:
        # 1. Crear un nuevo autobús de prueba
        nuevo_bus = Autobus(numero_unidad="BUS-001", placa="1234-ABC")
        
        # 2. Añadir y guardar en la base de datos
        db.add(nuevo_bus)
        db.commit()
        db.refresh(nuevo_bus)
        
        print(f"✅ Autobús registrado exitosamente: Unidad {nuevo_bus.numero_unidad}, Placa {nuevo_bus.placa}")

        # 3. Consultar toda la flota registrada
        buses = db.query(Autobus).all()
        print("\n🚌 Flota actual en el sistema:")
        for bus in buses:
            print(f"- ID: {bus.id} | Unidad: {bus.numero_unidad} | Placa: {bus.placa}")

    except Exception as e:
        print(f"❌ Ocurrió un error (posiblemente la unidad ya existe o la tabla no está creada): {e}")
        db.rollback()
    finally:
        # Siempre cerramos la sesión al terminar
        db.close()

if __name__ == "__main__":
    registrar_y_consultar()