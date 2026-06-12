#!/bin/bash

# Obtener directorio del script
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd "$DIR/P3CsharpOrm"

# Asegurar compatibilidad con .NET 10 (Roll Forward de dependencias de .NET 9)
export DOTNET_ROLL_FORWARD=Major
export PATH="$PATH:/home/raulito/.dotnet/tools"

show_menu() {
    echo "=================================================="
    echo "      GESTIÓN DE C# ORM - PRÁCTICA 7"
    echo "=================================================="
    echo "1) Aplicar migraciones a la Base de Datos"
    echo "2) Ejecutar la aplicación C# (dotnet run)"
    echo "3) Revertir migraciones (Volver a un estado anterior)"
    echo "4) Salir"
    echo "=================================================="
}

while true; do
    show_menu
    read -p "Elige una opción [1-4]: " opcion
    echo ""
    case $opcion in
        1)
            echo "--- Aplicando Migraciones (EF Core) ---"
            dotnet ef database update
            ;;
        2)
            echo "--- Ejecutando Aplicación C# ---"
            dotnet run
            ;;
        3)
            echo "--- Revertiendo Migraciones ---"
            echo "Escribe el nombre de la migración a la que deseas volver (o '0' para deshacer todas):"
            read -p "Nombre de la migración: " mig_name
            dotnet ef database update "$mig_name"
            ;;
        4)
            echo "¡Hasta luego!"
            exit 0
            ;;
        *)
            echo "Opción no válida. Por favor, selecciona una opción entre 1 y 4."
            ;;
    esac
    echo ""
    read -p "Presiona Enter para continuar..."
    clear
done
