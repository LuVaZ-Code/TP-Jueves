using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TP_Jueves.Models
{
    /// <summary>
    /// Represents a physical table in the restaurant.
    /// </summary>
    public class Mesa
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Seating capacity of the table (e.g., 2,4,6).
        /// </summary>
        [Range(1, 20)]
        public int Capacidad { get; set; }

        /// <summary>
        /// Navigation to reservations assigned to this mesa.
        /// </summary>
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
