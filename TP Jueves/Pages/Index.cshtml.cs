using Microsoft.AspNetCore.Mvc.RazorPages;
using TP_Jueves.Services;

namespace TP_Jueves.Pages
{
    /// <summary>
    /// PageModel for Index showing welcome modal with restaurant name.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly RestauranteService _restaurante;

        public IndexModel(RestauranteService restaurante)
        {
            _restaurante = restaurante;
        }

        public string RestaurantName { get; set; } = string.Empty;

        public void OnGet()
        {
            RestaurantName = _restaurante.GetNombre();
            // Add class to body so layout can target background only on this page if needed
            ViewData["BodyClass"] = "has-restaurant-bg";
        }
    }
}
