using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TP_Jueves.TempModels;

[Index("PropietarioId", Name = "IX_Restaurantes_PropietarioId")]
public partial class Restaurante
{
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PropietarioId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int IsDeleted { get; set; }

    public int ConfiguracionCompletada { get; set; }

    public int Estado { get; set; }

    [InverseProperty("Restaurante")]
    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();

    [ForeignKey("PropietarioId")]
    [InverseProperty("Restaurantes")]
    public virtual AspNetUser Propietario { get; set; } = null!;

    [InverseProperty("Restaurante")]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    [InverseProperty("Restaurante")]
    public virtual ICollection<TurnosDisponible> TurnosDisponibles { get; set; } = new List<TurnosDisponible>();
}
