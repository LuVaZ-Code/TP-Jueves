using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP_Jueves.Data;
using TP_Jueves.Models;

namespace TP_Jueves.Pages.Restaurants.Mesas
{
    [Authorize(Roles = "Restaurantero")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [BindProperty]
        public Mesa Mesa { get; set; } = new();

        public Restaurante? Restaurante { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            Mesa = await _db.Mesas
                .Include(m => m.Restaurante)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Mesa == null)
                return NotFound();

            Restaurante = Mesa.Restaurante;
            if (Restaurante == null)
                return NotFound();

            // Verificar que el usuario es el propietario
            if (Restaurante.PropietarioId != user.Id)
                return Forbid();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var mesa = await _db.Mesas
                .Include(m => m.Restaurante)
                .FirstOrDefaultAsync(m => m.Id == Mesa.Id);

            if (mesa == null)
                return NotFound();

            if (mesa.Restaurante == null || mesa.Restaurante.PropietarioId != user.Id)
                return Forbid();

            mesa.Capacidad = Mesa.Capacidad;
            await _db.SaveChangesAsync();

            return RedirectToPage("Index", new { restauranteId = mesa.RestauranteId });
        }
    }
}
