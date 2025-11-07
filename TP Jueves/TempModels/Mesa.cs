using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TP_Jueves.TempModels;

[Index("RestauranteId", Name = "IX_Mesas_RestauranteId")]
public partial class Mesa
{
    [Key]
    public int Id { get; set; }

    public int Capacidad { get; set; }

    public int RestauranteId { get; set; }

    [InverseProperty("Mesa")]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    [ForeignKey("RestauranteId")]
    [InverseProperty("Mesas")]
    public virtual Restaurante Restaurante { get; set; } = null!;
}
