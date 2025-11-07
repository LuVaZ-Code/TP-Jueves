using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TP_Jueves.TempModels;

public partial class TpjuevesContext : DbContext
{
    public TpjuevesContext()
    {
    }

    public TpjuevesContext(DbContextOptions<TpjuevesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Restaurante> Restaurantes { get; set; }

    public virtual DbSet<TurnosDisponible> TurnosDisponibles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=tpjueves.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasOne(d => d.Cliente).WithMany(p => p.Reservas).OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Mesa).WithMany(p => p.Reservas).OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Restaurante).WithMany(p => p.Reservas).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.TurnoDisponible).WithMany(p => p.Reservas).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Restaurante>(entity =>
        {
            entity.HasOne(d => d.Propietario).WithMany(p => p.Restaurantes).OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
