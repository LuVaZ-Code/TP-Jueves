using Microsoft.EntityFrameworkCore;
using TP_Jueves.Data;
using TP_Jueves.Models;

namespace TP_Jueves.Services
{
    /// <summary>
    /// Service implementing Restaurante behavior described in UML.
    /// Métodos: Reservar, BuscarMesa, VerReservaPorId, VerReservasPorDni.
    /// </summary>
    public class RestauranteService
    {
        private readonly ApplicationDbContext _db;
        private readonly string _nombre;

        public RestauranteService(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _nombre = configuration.GetValue<string>("Restaurant:Name") ?? "Mi Restaurante";
        }

        /// <summary>
        /// Returns the restaurant name.
        /// </summary>
        public string GetNombre()
        {
            return _nombre;
        }

        /// <summary>
        /// Busca una mesa disponible para la capacidad, fecha y horario.
        /// Strategy: best-fit (minimiza capacidad desperdiciada).
        /// Parameters:
        ///  - partySize: guests needed.
        ///  - fecha: date component (time ignored).
        ///  - horario: slot enum.
        /// Returns the Mesa if found, otherwise null.
        /// </summary>
        public async Task<Mesa?> BuscarMesaAsync(int partySize, DateTime fecha, Horario horario, CancellationToken cancellationToken = default)
        {
            // Normalize fecha to date-only (midnight)
            var targetDate = fecha.Date;

            // Load mesas and their reservations for the target slot
            var mesas = await _db.Mesas
                .Include(m => m.Reservas)
                .ToListAsync(cancellationToken);

            Mesa? best = null;
            int bestWaste = int.MaxValue;

            foreach (var mesa in mesas)
            {
                bool occupied = mesa.Reservas.Any(r => r.Fecha.Date == targetDate && r.Horario == horario);
                if (occupied)
                {
                    // No return/break; continue to next mesa
                    continue;
                }

                if (mesa.Capacidad >= partySize)
                {
                    int waste = mesa.Capacidad - partySize;
                    if (waste < bestWaste)
                    {
                        best = mesa;
                        bestWaste = waste;
                    }
                }
            }

            return best;
        }

        /// <summary>
        /// Attempts to create a reservation.
        /// If successful, returns Success=true and ReservaId.
        /// If not, Success=false and Suggestions populated.
        /// Single return pattern: use result variable.
        /// </summary>
        public async Task<ReservationResult> ReservarAsync(string dni, Dieta dieta, int partySize, DateTime fecha, Horario horario, CancellationToken cancellationToken = default)
        {
            var result = new ReservationResult();

            // Basic validation (argument checks)
            if (string.IsNullOrWhiteSpace(dni))
            {
                result.Success = false;
                result.Message = "DNI inválido.";
                return result;
            }

            if (partySize < 1)
            {
                result.Success = false;
                result.Message = "La cantidad de personas debe ser al menos 1.";
                return result;
            }

            // Find mesa
            var mesa = await BuscarMesaAsync(partySize, fecha, horario, cancellationToken);
            if (mesa != null)
            {
                // Create reserva
                var reserva = new Reserva
                {
                    DniCliente = dni.Trim(),
                    Dieta = dieta,
                    CantPersonas = partySize,
                    Fecha = fecha.Date,
                    Horario = horario,
                    MesaId = mesa.Id,
                    CreatedAt = DateTime.UtcNow
                };

                _db.Reservas.Add(reserva);
                await _db.SaveChangesAsync(cancellationToken);

                result.Success = true;
                result.ReservaId = reserva.Id;
                result.Message = "Reserva creada correctamente.";
                return result;
            }

            // No mesa available for requested slot — compute suggestions (same day other horarios first)
            var allHorarios = Enum.GetValues(typeof(Horario)).Cast<Horario>().ToList();

            foreach (var altHorario in allHorarios)
            {
                if (altHorario == horario) continue;
                var altMesa = await BuscarMesaAsync(partySize, fecha, altHorario, cancellationToken);
                if (altMesa != null)
                {
                    result.Suggestions.Add((fecha.Date, altHorario));
                    // Continue searching to provide multiple options; no break
                }
            }

            // If still no suggestions, check nearby days (+/- 1..3 days)
            if (!result.Suggestions.Any())
            {
                var offsets = new[] { 1, -1, 2, -2, 3, -3 };
                foreach (var offset in offsets)
                {
                    var d = fecha.Date.AddDays(offset);
                    foreach (var slot in allHorarios)
                    {
                        var altMesa = await BuscarMesaAsync(partySize, d, slot, cancellationToken);
                        if (altMesa != null)
                        {
                            result.Suggestions.Add((d, slot));
                        }
                    }
                    if (result.Suggestions.Count >= 6) break; // gather up to some suggestions
                }
            }

            result.Success = false;
            result.Message = "No hay disponibilidad para el turno solicitado.";
            return result;
        }

        /// <summary>
        /// Busca reserva por id.
        /// Returns null if not found.
        /// </summary>
        public async Task<Reserva?> VerReservaPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var reserva = await _db.Reservas.Include(r => r.Mesa).FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            return reserva;
        }

        /// <summary>
        /// Busca reservas por DNI.
        /// </summary>
        public async Task<List<Reserva>> VerReservasPorDniAsync(string dni, CancellationToken cancellationToken = default)
        {
            var norm = dni.Trim();
            var reservas = await _db.Reservas.Include(r => r.Mesa)
                .Where(r => r.DniCliente == norm)
                .OrderBy(r => r.Fecha).ThenBy(r => r.Horario)
                .ToListAsync(cancellationToken);
            return reservas;
        }
    }
}
