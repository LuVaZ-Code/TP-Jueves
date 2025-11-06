using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP_Jueves.Models;
using TP_Jueves.Services;

namespace TP_Jueves.Pages.Reservations
{
    [Authorize]
    public class ListModel : PageModel
    {
        private readonly RestauranteService _restaurante;
        private readonly UserManager<ApplicationUser> _userManager;

        public ListModel(RestauranteService restaurante, UserManager<ApplicationUser> userManager)
        {
            _restaurante = restaurante;
            _userManager = userManager;
        }

        public List<Reserva> Reservas { get; set; } = new();
        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Buscar por DNI si existe, si no, mostrar vacío
            // En futuro: vincular reservas a usuarios directamente
            Reservas = new List<Reserva>();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var reserva = await _restaurante.VerReservaPorIdAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            // Aquí se implementaría la lógica de cancelación
            Message = "Funcionalidad de cancelación en desarrollo.";
            return RedirectToPage();
        }
    }
}
