-- Migración: Agregar tabla HorariosRestaurante y campo HoraReserva en Reservas

-- 1. Crear tabla HorariosRestaurante
CREATE TABLE IF NOT EXISTS "HorariosRestaurante" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_HorariosRestaurante" PRIMARY KEY AUTOINCREMENT,
    "RestauranteId" INTEGER NOT NULL,
    "Hora" TEXT NOT NULL,
    "CapacidadMaxima" INTEGER NOT NULL,
    "EstaActivo" INTEGER NOT NULL,
    "DiasDisponibles" TEXT NULL,
    "Descripcion" TEXT NULL,
    "CreatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_HorariosRestaurante_Restaurantes_RestauranteId" FOREIGN KEY ("RestauranteId") REFERENCES "Restaurantes" ("Id") ON DELETE CASCADE
);

-- 2. Crear índice para HorariosRestaurante
CREATE INDEX IF NOT EXISTS "IX_HorariosRestaurante_RestauranteId" ON "HorariosRestaurante" ("RestauranteId");

-- 3. Agregar columna HoraReserva a la tabla Reservas (si no existe)
-- SQLite no permite ALTER COLUMN, así que creamos una nueva tabla y migramos datos

-- Primero, renombrar la tabla existente
ALTER TABLE "Reservas" RENAME TO "Reservas_Old";

-- Crear nueva tabla con la columna HoraReserva
CREATE TABLE "Reservas" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Reservas" PRIMARY KEY,
    "DniCliente" TEXT NOT NULL,
    "ClienteId" TEXT NULL,
    "Dieta" INTEGER NOT NULL,
    "CantPersonas" INTEGER NOT NULL,
    "Fecha" TEXT NOT NULL,
    "HoraReserva" TEXT NOT NULL DEFAULT '',
    "Horario" INTEGER NOT NULL,
    "RestauranteId" INTEGER NOT NULL,
    "MesaId" INTEGER NULL,
    "TurnoDisponibleId" INTEGER NULL,
    "CreatedAt" TEXT NOT NULL,
    "IsCancelled" INTEGER NOT NULL,
    CONSTRAINT "FK_Reservas_AspNetUsers_ClienteId" FOREIGN KEY ("ClienteId") REFERENCES "AspNetUsers" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Reservas_Mesas_MesaId" FOREIGN KEY ("MesaId") REFERENCES "Mesas" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Reservas_Restaurantes_RestauranteId" FOREIGN KEY ("RestauranteId") REFERENCES "Restaurantes" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Reservas_TurnosDisponibles_TurnoDisponibleId" FOREIGN KEY ("TurnoDisponibleId") REFERENCES "TurnosDisponibles" ("Id") ON DELETE SET NULL
);

-- Copiar datos de la tabla antigua a la nueva
INSERT INTO "Reservas" 
    ("Id", "DniCliente", "ClienteId", "Dieta", "CantPersonas", "Fecha", "HoraReserva", "Horario", 
     "RestauranteId", "MesaId", "TurnoDisponibleId", "CreatedAt", "IsCancelled")
SELECT 
    "Id", "DniCliente", "ClienteId", "Dieta", "CantPersonas", "Fecha", 
    CASE 
        WHEN "Horario" = 0 THEN '12:00'
        WHEN "Horario" = 1 THEN '13:30'
        WHEN "Horario" = 2 THEN '15:00'
        WHEN "Horario" = 3 THEN '20:00'
        WHEN "Horario" = 4 THEN '22:00'
        WHEN "Horario" = 5 THEN '00:00'
        ELSE '12:00'
    END as "HoraReserva",
    "Horario", "RestauranteId", "MesaId", "TurnoDisponibleId", "CreatedAt", "IsCancelled"
FROM "Reservas_Old";

-- Eliminar tabla antigua
DROP TABLE "Reservas_Old";

-- Recrear índices
CREATE INDEX IF NOT EXISTS "IX_Reservas_ClienteId" ON "Reservas" ("ClienteId");
CREATE INDEX IF NOT EXISTS "IX_Reservas_MesaId" ON "Reservas" ("MesaId");
CREATE INDEX IF NOT EXISTS "IX_Reservas_RestauranteId" ON "Reservas" ("RestauranteId");
CREATE INDEX IF NOT EXISTS "IX_Reservas_TurnoDisponibleId" ON "Reservas" ("TurnoDisponibleId");

-- 4. Insertar horarios por defecto para restaurantes existentes
INSERT INTO "HorariosRestaurante" ("RestauranteId", "Hora", "CapacidadMaxima", "EstaActivo", "Descripcion", "CreatedAt")
SELECT 
    r."Id",
    horario.hora,
    50,
    1,
    horario.desc,
    datetime('now')
FROM "Restaurantes" r
CROSS JOIN (
    SELECT '12:00' as hora, 'Almuerzo' as desc UNION ALL
    SELECT '12:30', 'Almuerzo' UNION ALL
    SELECT '13:00', 'Almuerzo' UNION ALL
    SELECT '13:30', 'Almuerzo' UNION ALL
    SELECT '20:00', 'Cena' UNION ALL
    SELECT '20:30', 'Cena' UNION ALL
    SELECT '21:00', 'Cena' UNION ALL
    SELECT '21:30', 'Cena'
) as horario
WHERE NOT EXISTS (
    SELECT 1 FROM "HorariosRestaurante" h 
    WHERE h."RestauranteId" = r."Id"
);
