using System.ComponentModel.DataAnnotations;

namespace TP_Jueves.Models
{
    /// <summary>
    /// Represents a restaurant business entity owned by a user.
    /// </summary>
    public class Restaurante
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [StringLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// FK to the owner/propietario of this restaurant.
        /// </summary>
        public string PropietarioId { get; set; } = string.Empty;
        public ApplicationUser? Propietario { get; set; }

        /// <summary>
        /// Navigation to mesas in this restaurant.
        /// </summary>
        public List<Mesa> Mesas { get; set; } = new();

        /// <summary>
        /// Navigation to reservations in this restaurant.
        /// </summary>
        public List<Reserva> Reservas { get; set; } = new();

        /// <summary>
        /// Creation timestamp in UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Soft delete flag.
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
