using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP_Jueves.Models
{
    /// <summary>
    /// Reservation record resulting from Restaurante.Reservar.
    /// </summary>
    public class Reserva
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// DNI del cliente (8 dígitos, stored as string to preserve leading zeros).
        /// </summary>
        [Required]
        [StringLength(8, MinimumLength = 8)]
        [RegularExpression("^\\d{8}$", ErrorMessage = "DNI debe contener exactamente 8 dígitos.")]
        public string DniCliente { get; set; } = string.Empty;

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
        /// Assigned mesa id and navigation.
        /// </summary>
        [ForeignKey(nameof(Mesa))]
        public int MesaId { get; set; }
        public Mesa? Mesa { get; set; }

        /// <summary>
        /// Created timestamp in UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
