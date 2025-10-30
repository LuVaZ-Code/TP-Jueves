using System;
using System.Collections.Generic;

namespace TP_Jueves.Services
{
    /// <summary>
    /// Result object for reservation attempts.
    /// Success = true -> Reserva created and ReservaId present.
    /// Success = false -> Suggestions may contain alternative slots.
    /// </summary>
    public class ReservationResult
    {
        public bool Success { get; set; }
        public Guid? ReservaId { get; set; }
        public string? Message { get; set; }
        public List<(DateTime fecha, TP_Jueves.Models.Horario horario)> Suggestions { get; set; } = new();
    }
}
