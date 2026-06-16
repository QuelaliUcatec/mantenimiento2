#!/bin/bash
echo "--- Panel de Control: P7-FleetControl ---"
echo "1. Aplicar migraciones (Update Database)"
echo "2. Ejecutar la aplicación (dotnet run)"
echo "3. Salir"
read -p "Seleccione una opción: " opcion

case $opcion in
    1) dotnet ef database update ;;
    2) dotnet run ;;
    3) exit ;;
    *) echo "Opción no válida" ;;
esac