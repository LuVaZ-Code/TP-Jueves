using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP_Jueves.Models
{
    /// <summary>
    /// Represents an available time slot for a restaurant.
    /// Allows managing which dates and times are available for bookings.
    /// </summary>
    public class TurnoDisponible
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// FK to the restaurant this turno belongs to.
        /// </summary>
        [ForeignKey(nameof(Restaurante))]
        public int RestauranteId { get; set; }
        public Restaurante? Restaurante { get; set; }

        /// <summary>
        /// Date of this turno (without time component).
        /// </summary>
        [Required]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// The specific time slot.
        /// </summary>
        [Required]
        public Horario Horario { get; set; }

        /// <summary>
        /// Maximum number of people that can reserve this slot.
        /// </summary>
        [Range(1, 100)]
        public int CapacidadMaxima { get; set; } = 50;

        /// <summary>
        /// Current number of people already reserved.
        /// </summary>
        [Range(0, 100)]
        public int CapacidadUsada { get; set; } = 0;

        /// <summary>
        /// Whether this turno is active/available.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Reservations in this slot.
        /// </summary>
        public List<Reserva> Reservas { get; set; } = new();

        /// <summary>
        /// Creation timestamp in UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Remaining capacity.
        /// </summary>
        [NotMapped]
        public int CapacidadRestante => CapacidadMaxima - CapacidadUsada;

        /// <summary>
        /// Is this slot fully booked?
        /// </summary>
        [NotMapped]
        public bool EstaLleno => CapacidadRestante <= 0;
    }
}
