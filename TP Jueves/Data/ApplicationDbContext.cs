using Microsoft.EntityFrameworkCore;
using TP_Jueves.Models;

namespace TP_Jueves.Data
{
    /// <summary>
    /// EF Core DB context containing Mesas y Reservas.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Mesa> Mesas { get; set; } = null!;
        public DbSet<Reserva> Reservas { get; set; } = null!;
    }
}
