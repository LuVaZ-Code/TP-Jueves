using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP_Jueves.Data;
using TP_Jueves.Models;

namespace TP_Jueves.Pages.Restaurants
{
    [Authorize(Roles = "Restaurantero")]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Restaurante? Restaurante { get; set; }
        public List<Reserva> ReservasHoy { get; set; } = new();
        public List<Reserva> ReservasProximas { get; set; } = new();
        public DashboardStats Stats { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            Restaurante = await _db.Restaurantes
                .Include(r => r.Mesas)
                .Include(r => r.Reservas.Where(res => !res.IsCancelled))
                    .ThenInclude(res => res.Cliente)
                .FirstOrDefaultAsync(r => r.Id == Id && r.PropietarioId == user.Id);

            if (Restaurante == null)
                return NotFound();

            // Si no está configurado, redirigir al wizard
            if (!Restaurante.ConfiguracionCompletada)
            {
                return RedirectToPage("/Restaurants/Setup/Wizard", new { id = Id, step = 2 });
            }

            var hoy = DateTime.Today;
            var ahora = DateTime.UtcNow;

            // Reservas de hoy
            ReservasHoy = Restaurante.Reservas
                .Where(r => r.Fecha.Date == hoy && !r.IsCancelled)
                .OrderBy(r => r.Horario)
                .ToList();

            // Próximas reservas (próximos 7 días)
            ReservasProximas = Restaurante.Reservas
                .Where(r => r.Fecha.Date > hoy && r.Fecha.Date <= hoy.AddDays(7) && !r.IsCancelled)
                .OrderBy(r => r.Fecha)
                .ThenBy(r => r.Horario)
                .Take(10)
                .ToList();

            // Estadísticas
            Stats.TotalMesas = Restaurante.Mesas.Count;
            Stats.TotalReservasActivas = Restaurante.Reservas.Count(r => !r.IsCancelled && r.Fecha >= hoy);
            Stats.ReservasHoy = ReservasHoy.Count;
            Stats.PersonasHoy = ReservasHoy.Sum(r => r.CantPersonas);
            Stats.TasaOcupacionHoy = CalcularTasaOcupacion(ReservasHoy);

            return Page();
        }

        public async Task<IActionResult> OnPostCambiarEstadoAsync(EstadoRestaurante nuevoEstado)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var restaurante = await _db.Restaurantes
                .FirstOrDefaultAsync(r => r.Id == Id && r.PropietarioId == user.Id);

            if (restaurante == null)
                return NotFound();

            restaurante.Estado = nuevoEstado;
            await _db.SaveChangesAsync();

            return RedirectToPage(new { id = Id });
        }

        private int CalcularTasaOcupacion(List<Reserva> reservas)
        {
            if (!reservas.Any())
                return 0;

            var turnosOcupados = reservas.Select(r => r.Horario).Distinct().Count();
            var turnosTotales = Enum.GetValues(typeof(Horario)).Length;

            return (int)((double)turnosOcupados / turnosTotales * 100);
        }

        public class DashboardStats
        {
            public int TotalMesas { get; set; }
            public int TotalReservasActivas { get; set; }
            public int ReservasHoy { get; set; }
            public int PersonasHoy { get; set; }
            public int TasaOcupacionHoy { get; set; }
        }
    }
}
