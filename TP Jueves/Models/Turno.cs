using System;
namespace TP_Jueves.Models
{
    /// <summary>
    /// Value object representing a turno: date + horario.
    /// </summary>
    public class Turno
    {
        /// <summary>
        /// Date of the turno (date component only).
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Horario enum for the slot.
        /// </summary>
        public Horario Horario { get; set; }
    }
}
