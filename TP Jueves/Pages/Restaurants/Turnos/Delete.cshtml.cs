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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public TurnoDisponible? TurnoDisponible { get; set; }
        public Restaurante? Restaurante { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            TurnoDisponible = await _db.TurnosDisponibles
                .Include(t => t.Restaurante)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (TurnoDisponible == null)
                return NotFound();

            Restaurante = TurnoDisponible.Restaurante;
            if (Restaurante == null)
                return NotFound();

            // Verificar que el usuario es el propietario
            if (Restaurante.PropietarioId != user.Id)
                return Forbid();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var turno = await _db.TurnosDisponibles
                .Include(t => t.Restaurante)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (turno == null)
                return NotFound();

            if (turno.Restaurante == null || turno.Restaurante.PropietarioId != user.Id)
                return Forbid();

            var restauranteId = turno.RestauranteId;
            turno.IsActive = false;
            await _db.SaveChangesAsync();

            return RedirectToPage("Index", new { restauranteId = restauranteId });
        }
    }
}
