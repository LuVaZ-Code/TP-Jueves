using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP_Jueves.Data;
using TP_Jueves.Models;

namespace TP_Jueves.Pages.Restaurants.Turnos
{
    [Authorize(Roles = "Restaurantero")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public Restaurante? Restaurante { get; set; }
        public List<HorarioRestaurante> Horarios { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int restauranteId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            Restaurante = await _db.Restaurantes
                .Include(r => r.Horarios)
                .FirstOrDefaultAsync(r => r.Id == restauranteId);
            
            if (Restaurante == null)
                return NotFound();

            // Verificar que el usuario es el propietario
            if (Restaurante.PropietarioId != user.Id)
                return Forbid();

            Horarios = Restaurante.Horarios
                .OrderBy(h => h.Hora)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostToggleAsync(int id)
        {
            var horario = await _db.HorariosRestaurante.FindAsync(id);
            if (horario == null)
                return NotFound();

            // Verificar propiedad del restaurante
            var user = await _userManager.GetUserAsync(User);
            var restaurante = await _db.Restaurantes.FindAsync(horario.RestauranteId);
            if (restaurante?.PropietarioId != user?.Id)
                return Forbid();

            horario.EstaActivo = !horario.EstaActivo;
            await _db.SaveChangesAsync();

            return RedirectToPage(new { restauranteId = horario.RestauranteId });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var horario = await _db.HorariosRestaurante.FindAsync(id);
            if (horario == null)
                return NotFound();

            // Verificar propiedad del restaurante
            var user = await _userManager.GetUserAsync(User);
            var restaurante = await _db.Restaurantes.FindAsync(horario.RestauranteId);
            if (restaurante?.PropietarioId != user?.Id)
                return Forbid();

            _db.HorariosRestaurante.Remove(horario);
            await _db.SaveChangesAsync();

            return RedirectToPage(new { restauranteId = horario.RestauranteId });
        }
    }
}
