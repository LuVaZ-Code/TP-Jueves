using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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
        public HorarioInput Input { get; set; } = new();

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Restaurante = await _db.Restaurantes.FirstOrDefaultAsync(r => r.Id == RestauranteId);
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            Restaurante = await _db.Restaurantes.FirstOrDefaultAsync(r => r.Id == RestauranteId);
            if (Restaurante == null)
                return NotFound();

            // Verificar que el usuario es el propietario
            if (Restaurante.PropietarioId != user.Id)
                return Forbid();

            // Verificar si ya existe ese horario
            var existe = await _db.HorariosRestaurante
                .AnyAsync(h => h.RestauranteId == RestauranteId && h.Hora == Input.Hora);

            if (existe)
            {
                ModelState.AddModelError(string.Empty, $"Ya existe un horario para las {Input.Hora}");
                return Page();
            }

            var nuevoHorario = new HorarioRestaurante
            {
                RestauranteId = RestauranteId,
                Hora = Input.Hora,
                Descripcion = Input.Descripcion,
                EstaActivo = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.HorariosRestaurante.Add(nuevoHorario);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index", new { restauranteId = RestauranteId });
        }

        public class HorarioInput
        {
            [Required(ErrorMessage = "La hora es requerida")]
            [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Formato de hora inválido (HH:MM)")]
            public string Hora { get; set; } = string.Empty;

            [StringLength(50)]
            public string? Descripcion { get; set; }
        }
    }
}
