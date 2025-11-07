using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace MigracionHorarios
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.WriteLine("  APLICANDO MIGRACION: HorariosRestaurante");
            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.WriteLine();

            var dbPath = @"C:\Users\losmelli\Source\Repos\TP-Jueves\TP Jueves\tpjueves.db";
            var sqlFile = @"C:\Users\losmelli\Source\Repos\TP-Jueves\migration_horarios.sql";

            if (!File.Exists(dbPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: No se encontró la base de datos en: {dbPath}");
                Console.ResetColor();
                return;
            }

            if (!File.Exists(sqlFile))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: No se encontró el archivo SQL en: {sqlFile}");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Base de datos encontrada: {dbPath}");
            Console.WriteLine($"Archivo SQL encontrado: {sqlFile}");
            Console.ResetColor();
            Console.WriteLine();

            // Crear backup
            var backupPath = $"{dbPath}.backup_{DateTime.Now:yyyyMMdd_HHmmss}";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Creando backup en: {backupPath}");
            Console.ResetColor();
            File.Copy(dbPath, backupPath, true);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Backup creado exitosamente!");
            Console.ResetColor();
            Console.WriteLine();

            // Leer SQL
            var sqlContent = File.ReadAllText(sqlFile);
            var sqlStatements = sqlContent.Split(new[] { ";\r\n", ";\n" }, StringSplitOptions.RemoveEmptyEntries);

            // Aplicar migración
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Aplicando migración...");
            Console.ResetColor();

            try
            {
                using var connection = new SqliteConnection($"Data Source={dbPath}");
                connection.Open();

                foreach (var sql in sqlStatements)
                {
                    var trimmedSql = sql.Trim();
                    if (string.IsNullOrWhiteSpace(trimmedSql) || trimmedSql.StartsWith("--"))
                        continue;

                    using var command = connection.CreateCommand();
                    command.CommandText = trimmedSql;
                    command.ExecuteNonQuery();
                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("==================================================");
                Console.WriteLine("  MIGRACION APLICADA EXITOSAMENTE!");
                Console.WriteLine("==================================================");
                Console.WriteLine();
                Console.WriteLine("La tabla HorariosRestaurante ha sido creada.");
                Console.WriteLine("Se agregaron horarios por defecto a todos los restaurantes.");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Backup guardado en: {backupPath}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR al aplicar la migración: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Restaurando backup...");
                Console.ResetColor();
                File.Copy(backupPath, dbPath, true);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Base de datos restaurada desde el backup.");
                Console.ResetColor();
                throw;
            }

            Console.WriteLine();
            Console.WriteLine("Presiona cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
