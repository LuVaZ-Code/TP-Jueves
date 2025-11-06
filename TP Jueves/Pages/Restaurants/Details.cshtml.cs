using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP_Jueves.Data;
using TP_Jueves.Models;

namespace TP_Jueves.Pages.Restaurants
{
    /// <summary>
    /// Public page showing restaurant details and available tables.
    /// </summary>
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DetailsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public Restaurante? Restaurante { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Restaurante = await _db.Restaurantes
                .Include(r => r.Mesas)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (Restaurante == null)
                return NotFound();

            return Page();
        }
    }
}
