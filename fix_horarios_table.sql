-- Migración: Eliminar columna CapacidadMaxima de HorariosRestaurante

-- SQLite no permite DROP COLUMN, así que recreamos la tabla

-- 1. Renombrar tabla existente
ALTER TABLE HorariosRestaurante RENAME TO HorariosRestaurante_Old;

-- 2. Crear nueva tabla sin CapacidadMaxima
CREATE TABLE HorariosRestaurante (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    RestauranteId INTEGER NOT NULL,
    Hora TEXT NOT NULL,
    EstaActivo INTEGER NOT NULL,
    Descripcion TEXT NULL,
    CreatedAt TEXT NOT NULL,
    FOREIGN KEY (RestauranteId) REFERENCES Restaurantes(Id) ON DELETE CASCADE
);

-- 3. Copiar datos (sin CapacidadMaxima)
INSERT INTO HorariosRestaurante (Id, RestauranteId, Hora, EstaActivo, Descripcion, CreatedAt)
SELECT Id, RestauranteId, Hora, EstaActivo, Descripcion, CreatedAt
FROM HorariosRestaurante_Old;

-- 4. Eliminar tabla antigua
DROP TABLE HorariosRestaurante_Old;

-- 5. Recrear índice
CREATE INDEX IF NOT EXISTS IX_HorariosRestaurante_RestauranteId ON HorariosRestaurante (RestauranteId);
