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
        /// DNI del cliente (número, hasta 8 dígitos).
        /// </summary>
        [Required]
        public int DniCliente { get; set; }

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
        /// Fecha (date only) del turno almacenada en UTC date kind for consistency.
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
