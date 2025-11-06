using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP_Jueves.Data;
using TP_Jueves.Models;

namespace TP_Jueves.Pages.Restaurants
{
    /// <summary>
    /// Public page listing all available restaurants for clients to browse.
    /// </summary>
    public class BrowseModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public BrowseModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Restaurante> Restaurantes { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public async Task OnGetAsync()
        {
            var query = _db.Restaurantes
                .Where(r => !r.IsDeleted)
                .OrderBy(r => r.Nombre)
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(r => 
                    r.Nombre.Contains(SearchTerm) || 
                    r.Descripcion.Contains(SearchTerm) ||
                    r.Direccion.Contains(SearchTerm));
            }

            Restaurantes = await query.Include(r => r.Mesas).ToListAsync();
        }
    }
}
