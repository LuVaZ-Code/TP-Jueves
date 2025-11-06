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
        public List<Mesa> Mesas { get; set; } = new();

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

            Mesas = await _db.Mesas
                .Where(m => m.RestauranteId == restauranteId)
                .OrderBy(m => m.Capacidad)
                .ToListAsync();

            return Page();
        }
    }
}
