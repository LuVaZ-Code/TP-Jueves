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
        public Restaurante Restaurante { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            Restaurante = await _db.Restaurantes.FirstOrDefaultAsync(r => r.Id == id);
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

            var restaurante = await _db.Restaurantes.FirstOrDefaultAsync(r => r.Id == Restaurante.Id);
            if (restaurante == null)
                return NotFound();

            // Verificar que el usuario es el propietario
            if (restaurante.PropietarioId != user.Id)
                return Forbid();

            restaurante.Nombre = Restaurante.Nombre;
            restaurante.Descripcion = Restaurante.Descripcion;
            restaurante.Direccion = Restaurante.Direccion;
            restaurante.Telefono = Restaurante.Telefono;
            restaurante.Email = Restaurante.Email;

            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
