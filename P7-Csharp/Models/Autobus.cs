using System;

namespace P7_FleetControl.Models;

public class Autobus
{
    public int Id { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public int Capacidad { get; set; }
}