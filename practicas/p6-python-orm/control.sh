#!/bin/bash

# Obtener directorio del script para evitar problemas de rutas
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd "$DIR"

# Rutas a los binarios del entorno virtual
VENV_ALEMBIC="./venv/bin/alembic"
VENV_PYTHON="./venv/bin/python"

# Verificar si el venv existe
if [ ! -f "$VENV_ALEMBIC" ]; then
    echo "Error: No se encontró el entorno virtual en ./venv."
    echo "Asegúrate de haber instalado los requerimientos primero."
    exit 1
fi

show_menu() {
    echo "=================================================="
    echo "      GESTIÓN DE MIGRACIONES - PRÁCTICA 6"
    echo "=================================================="
    echo "1) Ver revisión actual en la BD (alembic current)"
    echo "2) Ver historial de todas las migraciones (alembic history)"
    echo "3) Aplicar todas las migraciones pendientes (alembic upgrade head)"
    echo "4) Deshacer la última migración (alembic downgrade -1)"
    echo "5) Ejecutar script de prueba (main.py)"
    echo "6) Salir"
    echo "=================================================="
}

while true; do
    show_menu
    read -p "Elige una opción [1-6]: " opcion
    echo ""
    case $opcion in
        1)
            echo "--- Revisión Actual ---"
            $VENV_ALEMBIC current
            ;;
        2)
            echo "--- Historial de Migraciones ---"
            $VENV_ALEMBIC history --verbose
            ;;
        3)
            echo "--- Aplicando todas las migraciones (Upgrade) ---"
            $VENV_ALEMBIC upgrade head
            ;;
        4)
            echo "--- Deshaciendo última migración (Downgrade -1) ---"
            $VENV_ALEMBIC downgrade -1
            ;;
        5)
            echo "--- Ejecutando script de prueba (main.py) ---"
            $VENV_PYTHON main.py
            ;;
        6)
            echo "¡Hasta luego!"
            exit 0
            ;;
        *)
            echo "Opción no válida. Por favor, selecciona una opción entre 1 y 6."
            ;;
    esac
    echo ""
    read -p "Presiona Enter para continuar..."
    clear
done
