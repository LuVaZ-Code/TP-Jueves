using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP_Jueves.Models
{
    /// <summary>
    /// Reservation record linking Cliente, Restaurante, Mesa, and timing info.
    /// </summary>
    public class Reserva
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// DNI del cliente (8 digitos, stored as string to preserve leading zeros).
        /// </summary>
        [Required]
        [StringLength(8, MinimumLength = 8)]
        [RegularExpression("^\\d{8}$", ErrorMessage = "DNI debe contener exactamente 8 digitos.")]
        public string DniCliente { get; set; } = string.Empty;

        /// <summary>
        /// FK to the client/user making the reservation.
        /// </summary>
        public string? ClienteId { get; set; }
        public ApplicationUser? Cliente { get; set; }

        /// <summary>
        /// Dieta solicitada.
        /// </summary>
        public Dieta Dieta { get; set; }

        /// <summary>
        /// Cantidad de personas.
        /// </summary>
        [Range(1, 20)]
        public int CantPersonas { get; set; }

        /// <summary>
        /// Fecha (date only) del turno almacenada in UTC for consistency.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Horario del turno.
        /// </summary>
        public Horario Horario { get; set; }

        /// <summary>
        /// FK to the restaurant where reservation is made.
        /// </summary>
        [ForeignKey(nameof(Restaurante))]
        public int RestauranteId { get; set; }
        public Restaurante? Restaurante { get; set; }

        /// <summary>
        /// Assigned mesa id and navigation.
        /// </summary>
        [ForeignKey(nameof(Mesa))]
        public int? MesaId { get; set; }
        public Mesa? Mesa { get; set; }

        /// <summary>
        /// FK to the available turno (optional, for future tracking).
        /// </summary>
        [ForeignKey(nameof(TurnoDisponible))]
        public int? TurnoDisponibleId { get; set; }
        public TurnoDisponible? TurnoDisponible { get; set; }

        /// <summary>
        /// Created timestamp in UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Soft delete / cancellation flag.
        /// </summary>
        public bool IsCancelled { get; set; } = false;
    }
}
