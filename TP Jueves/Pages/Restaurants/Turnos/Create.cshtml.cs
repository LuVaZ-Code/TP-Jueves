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
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [BindProperty]
        public TurnoDisponible TurnoDisponible { get; set; } = new();

        [BindProperty]
        public int RestauranteId { get; set; }

        public Restaurante? Restaurante { get; set; }

        public async Task<IActionResult> OnGetAsync(int restauranteId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            Restaurante = await _db.Restaurantes.FirstOrDefaultAsync(r => r.Id == restauranteId);
            if (Restaurante == null)
                return NotFound();

            // Verificar que el usuario es el propietario
            if (Restaurante.PropietarioId != user.Id)
                return Forbid();

            RestauranteId = restauranteId;
            TurnoDisponible.Fecha = DateTime.Today;
            TurnoDisponible.CapacidadMaxima = 50;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            Restaurante = await _db.Restaurantes.FirstOrDefaultAsync(r => r.Id == RestauranteId);
            if (Restaurante == null)
                return NotFound();

            // Verificar que el usuario es el propietario
            if (Restaurante.PropietarioId != user.Id)
                return Forbid();

            TurnoDisponible.RestauranteId = RestauranteId;
            TurnoDisponible.CreatedAt = DateTime.UtcNow;
            TurnoDisponible.CapacidadUsada = 0;

            _db.TurnosDisponibles.Add(TurnoDisponible);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index", new { restauranteId = RestauranteId });
        }
    }
}
