using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP_Jueves.Data;
using TP_Jueves.Models;
using TP_Jueves.Services;

namespace TP_Jueves.Pages.Reservations
{
    [Authorize(Roles = "Cliente")]
    public class ListModel : PageModel
    {
        private readonly RestauranteService _restaurante;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public ListModel(RestauranteService restaurante, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _restaurante = restaurante;
            _userManager = userManager;
            _db = db;
        }

        public List<Reserva> Reservas { get; set; } = new();
        public string? Message { get; set; }
        public ApplicationUser? CurrentUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string? successMsg, string? errorMsg)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(successMsg))
                Message = successMsg;
            else if (!string.IsNullOrEmpty(errorMsg))
                Message = errorMsg;

            // Buscar reservas del cliente autenticado (no canceladas)
            Reservas = await _db.Reservas
                .Where(r => r.ClienteId == CurrentUser.Id && !r.IsCancelled)
                .Include(r => r.Mesa)
                .Include(r => r.Restaurante)
                .OrderByDescending(r => r.Fecha)
                .ThenBy(r => r.Horario)
                .ToListAsync();

            return Page();
        }
    }
}
