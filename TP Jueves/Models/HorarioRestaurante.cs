using System.ComponentModel.DataAnnotations;

namespace TP_Jueves.Models
{
    /// <summary>
    /// Representa un horario específico configurado por el restaurante.
    /// Por ejemplo: "12:00", "13:00", "20:00", etc.
    /// La disponibilidad real se calcula en base a las mesas físicas del restaurante.
    /// </summary>
    public class HorarioRestaurante
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// ID del restaurante al que pertenece este horario.
        /// </summary>
        public int RestauranteId { get; set; }
        public Restaurante? Restaurante { get; set; }

        /// <summary>
        /// Hora en formato "HH:mm" (ej: "12:00", "20:30")
        /// </summary>
        [Required]
        [StringLength(5)]
        public string Hora { get; set; } = string.Empty;

        /// <summary>
        /// Indica si este horario está activo/disponible.
        /// </summary>
        public bool EstaActivo { get; set; } = true;

        /// <summary>
        /// Descripción del horario (ej: "Almuerzo", "Cena")
        /// </summary>
        [StringLength(50)]
        public string? Descripcion { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
