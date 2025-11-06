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

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
            {
                return NotFound();
            }

            // Buscar reservas del cliente autenticado
            Reservas = await _db.Reservas
                .Where(r => r.ClienteId == CurrentUser.Id && !r.IsCancelled)
                .Include(r => r.Mesa)
                .Include(r => r.Restaurante)
                .OrderByDescending(r => r.Fecha)
                .ThenBy(r => r.Horario)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var reserva = await _db.Reservas.FirstOrDefaultAsync(r => r.Id == id);
            if (reserva == null)
                return NotFound();

            // Verificar que es su reserva
            if (reserva.ClienteId != user.Id)
                return Forbid();

            // Verificar que no sea una reserva del pasado
            if (reserva.Fecha.Date < DateTime.Today)
            {
                Message = "No se puede cancelar reservas pasadas.";
                return RedirectToPage();
            }

            reserva.IsCancelled = true;
            await _db.SaveChangesAsync();

            Message = "Reserva cancelada correctamente.";
            return RedirectToPage();
        }
    }
}
