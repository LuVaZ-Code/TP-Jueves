using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TP_Jueves.Models;

namespace TP_Jueves.Data
{
    /// <summary>
    /// EF Core DB context containing Restaurantes, Mesas, Reservas and Identity users.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Restaurante> Restaurantes { get; set; } = null!;
        public DbSet<Mesa> Mesas { get; set; } = null!;
        public DbSet<TurnoDisponible> TurnosDisponibles { get; set; } = null!;
        public DbSet<Reserva> Reservas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Restaurante -> ApplicationUser (Propietario) relationship
            modelBuilder.Entity<Restaurante>()
                .HasOne(r => r.Propietario)
                .WithMany()
                .HasForeignKey(r => r.PropietarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Mesa -> Restaurante relationship
            modelBuilder.Entity<Mesa>()
                .HasOne(m => m.Restaurante)
                .WithMany(r => r.Mesas)
                .HasForeignKey(m => m.RestauranteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure TurnoDisponible -> Restaurante relationship
            modelBuilder.Entity<TurnoDisponible>()
                .HasOne(t => t.Restaurante)
                .WithMany()
                .HasForeignKey(t => t.RestauranteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Reserva -> Restaurante relationship
            modelBuilder.Entity<Reserva>()
                .HasOne(res => res.Restaurante)
                .WithMany(r => r.Reservas)
                .HasForeignKey(res => res.RestauranteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Reserva -> Mesa relationship
            modelBuilder.Entity<Reserva>()
                .HasOne(res => res.Mesa)
                .WithMany(m => m.Reservas)
                .HasForeignKey(res => res.MesaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Reserva -> TurnoDisponible relationship
            modelBuilder.Entity<Reserva>()
                .HasOne(res => res.TurnoDisponible)
                .WithMany(t => t.Reservas)
                .HasForeignKey(res => res.TurnoDisponibleId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Reserva -> Cliente (ApplicationUser) relationship
            modelBuilder.Entity<Reserva>()
                .HasOne(res => res.Cliente)
                .WithMany()
                .HasForeignKey(res => res.ClienteId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
