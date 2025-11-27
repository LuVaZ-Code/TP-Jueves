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
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == Id && r.PropietarioId == user.Id);

            if (Restaurante == null)
                return NotFound();

            // Si no est� configurado, redirigir al wizard
            if (!Restaurante.ConfiguracionCompletada)
            {
                return RedirectToPage("/Restaurants/Setup/Wizard", new { id = Id, step = 2 });
            }

            var hoy = DateTime.Today;

            // Cargar reservas no canceladas del restaurante
            var todasLasReservas = await _db.Reservas
                .Where(r => r.RestauranteId == Id && !r.IsCancelled)
                .Include(r => r.Cliente)
                .AsNoTracking()
                .ToListAsync();

            // Reservas de hoy
            ReservasHoy = todasLasReservas
                .Where(r => r.Fecha.Date == hoy)
                .OrderBy(r => r.Horario)
                .ToList();

            // Pr�ximas reservas (pr�ximos 7 d�as)
            ReservasProximas = todasLasReservas
                .Where(r => r.Fecha.Date > hoy && r.Fecha.Date <= hoy.AddDays(7))
                .OrderBy(r => r.Fecha)
                .ThenBy(r => r.Horario)
                .Take(10)
                .ToList();

            // Estad�sticas
            Stats.TotalMesas = Restaurante.Mesas.Count;
            Stats.TotalReservasActivas = todasLasReservas.Count(r => r.Fecha >= hoy);
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

        public async Task<IActionResult> OnPostEliminarReservaAsync(Guid reservaId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            // Don't use AsNoTracking here - we need to track changes
            var reserva = await _db.Reservas
                .Include(r => r.Restaurante)
                .FirstOrDefaultAsync(r => r.Id == reservaId && r.Restaurante.PropietarioId == user.Id && !r.IsCancelled);

            if (reserva == null)
                return NotFound();

            // Cancelar la reserva en lugar de eliminarla
            reserva.IsCancelled = true;
            
            // Ensure the change is tracked and saved
            _db.Reservas.Update(reserva);
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
