using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TP_Jueves.TempModels;

[Index("ClienteId", Name = "IX_Reservas_ClienteId")]
[Index("MesaId", Name = "IX_Reservas_MesaId")]
[Index("RestauranteId", Name = "IX_Reservas_RestauranteId")]
[Index("TurnoDisponibleId", Name = "IX_Reservas_TurnoDisponibleId")]
public partial class Reserva
{
    [Key]
    public string Id { get; set; } = null!;

    public int CantPersonas { get; set; }

    public string? ClienteId { get; set; }

    public string CreatedAt { get; set; } = null!;

    public int Dieta { get; set; }

    public string DniCliente { get; set; } = null!;

    public string Fecha { get; set; } = null!;

    public int Horario { get; set; }

    public int IsCancelled { get; set; }

    public int? MesaId { get; set; }

    public int RestauranteId { get; set; }

    public int? TurnoDisponibleId { get; set; }

    [ForeignKey("ClienteId")]
    [InverseProperty("Reservas")]
    public virtual AspNetUser? Cliente { get; set; }

    [ForeignKey("MesaId")]
    [InverseProperty("Reservas")]
    public virtual Mesa? Mesa { get; set; }

    [ForeignKey("RestauranteId")]
    [InverseProperty("Reservas")]
    public virtual Restaurante Restaurante { get; set; } = null!;

    [ForeignKey("TurnoDisponibleId")]
    [InverseProperty("Reservas")]
    public virtual TurnosDisponible? TurnoDisponible { get; set; }
}
