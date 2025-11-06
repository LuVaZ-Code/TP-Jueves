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
    public class CancelModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CancelModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
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
                .FirstOrDefaultAsync(r => r.Id == id);

            if (Reserva == null)
                return NotFound();

            // Verificar que es su reserva
            if (Reserva.ClienteId != user.Id)
                return Forbid();

            // Verificar que no sea pasada
            if (Reserva.Fecha.Date < DateTime.Today)
                return RedirectToPage("List", new { errorMsg = "No se puede cancelar reservas pasadas" });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var reserva = await _db.Reservas
                .Include(r => r.Restaurante)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            // Verificar que es su reserva
            if (reserva.ClienteId != user.Id)
                return Forbid();

            // Verificar que no sea pasada
            if (reserva.Fecha.Date < DateTime.Today)
                return RedirectToPage("List", new { errorMsg = "No se puede cancelar reservas pasadas" });

            reserva.IsCancelled = true;
            await _db.SaveChangesAsync();

            return RedirectToPage("List", new { successMsg = "Reserva cancelada correctamente" });
        }
    }
}
