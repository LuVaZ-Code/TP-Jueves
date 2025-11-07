using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TP_Jueves.TempModels;

[Index("RestauranteId", Name = "IX_TurnosDisponibles_RestauranteId")]
public partial class TurnosDisponible
{
    [Key]
    public int Id { get; set; }

    public int RestauranteId { get; set; }

    public string Fecha { get; set; } = null!;

    public int Horario { get; set; }

    public int CapacidadMaxima { get; set; }

    public int CapacidadUsada { get; set; }

    public int IsActive { get; set; }

    public string CreatedAt { get; set; } = null!;

    [InverseProperty("TurnoDisponible")]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    [ForeignKey("RestauranteId")]
    [InverseProperty("TurnosDisponibles")]
    public virtual Restaurante Restaurante { get; set; } = null!;
}
