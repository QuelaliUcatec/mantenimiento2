using System;
using P7_FleetControl.Data;
using P7_FleetControl.Models;

Console.WriteLine("🚀 Iniciando conector ORM - P7-FleetControl...");

using (var db = new AppDbContext())
{
    // Verifica si la base de datos existe y se puede conectar
    bool canConnect = await db.Database.CanConnectAsync();
    
    if (canConnect)
    {
        Console.WriteLine("✅ ¡Conexión exitosa con la Base de Datos!");
        
        // Ejemplo: Agregar un Autobus de prueba
        var bus = new Autobus { Marca = "Mercedes", Modelo = "Tourismo", Placa = "1234-ABC", Capacidad = 50 };
        db.Autobuses.Add(bus);
        await db.SaveChangesAsync();
        
        Console.WriteLine($" Autobus {bus.Marca} guardado correctamente.");
    }
    else
    {
        Console.WriteLine("❌ No se pudo conectar a la base de datos.");
    }
}

Console.WriteLine(" Finalizando ejecución.");