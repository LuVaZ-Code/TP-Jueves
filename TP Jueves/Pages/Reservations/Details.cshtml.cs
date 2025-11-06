using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP_Jueves.Data;
using TP_Jueves.Models;

namespace TP_Jueves.Pages.Reservations
{
    [Authorize(Roles = "Cliente")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public Reserva? Reserva { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            Reserva = await _db.Reservas
                .Include(r => r.Mesa)
                .Include(r => r.Restaurante)
                .Include(r => r.TurnoDisponible)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (Reserva == null)
                return NotFound();

            // Verificar que es su reserva
            if (Reserva.ClienteId != user.Id)
                return Forbid();

            return Page();
        }
    }
}
