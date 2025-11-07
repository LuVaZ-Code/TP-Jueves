using Microsoft.Data.Sqlite;
using System;
using System.IO;

// Script simple para aplicar migración
var dbPath = @"C:\Users\losmelli\Source\Repos\TP-Jueves\TP Jueves\tpjueves.db";

Console.WriteLine("Aplicando migración...");

using var connection = new SqliteConnection($"Data Source={dbPath}");
connection.Open();

// Crear tabla HorariosRestaurante
var createTable = @"
CREATE TABLE IF NOT EXISTS HorariosRestaurante (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    RestauranteId INTEGER NOT NULL,
    Hora TEXT NOT NULL,
    CapacidadMaxima INTEGER NOT NULL,
    EstaActivo INTEGER NOT NULL,
    DiasDisponibles TEXT NULL,
    Descripcion TEXT NULL,
    CreatedAt TEXT NOT NULL,
    FOREIGN KEY (RestauranteId) REFERENCES Restaurantes(Id) ON DELETE CASCADE
)";

using (var cmd = connection.CreateCommand())
{
    cmd.CommandText = createTable;
    cmd.ExecuteNonQuery();
}

Console.WriteLine("Tabla HorariosRestaurante creada!");

// Agregar columna HoraReserva a Reservas
try
{
    using var cmd = connection.CreateCommand();
    cmd.CommandText = "ALTER TABLE Reservas ADD COLUMN HoraReserva TEXT NOT NULL DEFAULT ''";
    cmd.ExecuteNonQuery();
    Console.WriteLine("Columna HoraReserva agregada!");
}
catch
{
    Console.WriteLine("Columna HoraReserva ya existe.");
}

// Insertar horarios por defecto
var insertHorarios = @"
INSERT INTO HorariosRestaurante (RestauranteId, Hora, CapacidadMaxima, EstaActivo, Descripcion, CreatedAt)
SELECT 
    r.Id,
    '12:00',
    50,
    1,
    'Almuerzo',
    datetime('now')
FROM Restaurantes r
WHERE NOT EXISTS (SELECT 1 FROM HorariosRestaurante h WHERE h.RestauranteId = r.Id AND h.Hora = '12:00')";

using (var cmd = connection.CreateCommand())
{
    cmd.CommandText = insertHorarios;
    cmd.ExecuteNonQuery();
}

Console.WriteLine("Migración completada!");
