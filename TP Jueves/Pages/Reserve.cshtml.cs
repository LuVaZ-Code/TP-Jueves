using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
using TP_Jueves.Models;
using TP_Jueves.Services;

namespace TP_Jueves.Pages
{
    /// <summary>
    /// PageModel for reservation form.
    /// Handles GET (form) and POST (attempt to reservar).
    /// </summary>
    public class ReserveModel : PageModel
    {
        private readonly RestauranteService _restaurante;

        public ReserveModel(RestauranteService restaurante)
        {
            _restaurante = restaurante;
        }

        [BindProperty]
        public int Dni { get; set; }

        [BindProperty]
        public Dieta Dieta { get; set; } = Dieta.Normal;

        [BindProperty]
        public int CantPersonas { get; set; } = 1;

        [BindProperty]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [BindProperty]
        public Horario Horario { get; set; } = Horario.H20_22;

        public string? Message { get; set; }
        public Guid? ReservaId { get; set; }
        public List<(DateTime fecha, Horario horario)> Suggestions { get; set; } = new();

        public void OnGet()
        {
            // Ensure the layout will apply the restaurant background
            ViewData["BodyClass"] = "has-restaurant-bg";
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
        {
            // Server-side validation: DNI must be 8 digits (numeric)
            if (Dni < 10000000 || Dni > 99999999)
            {
                ModelState.AddModelError(nameof(Dni), "DNI debe contener exactamente 8 dígitos numéricos.");
            }

            // Fecha must be today or future
            var today = DateTime.Today;
            if (Fecha.Date < today)
            {
                ModelState.AddModelError(nameof(Fecha), "La fecha debe ser hoy o una fecha futura.");
            }

            if (CantPersonas < 1)
            {
                ModelState.AddModelError(nameof(CantPersonas), "Debe indicar al menos 1 comensal.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _restaurante.ReservarAsync(Dni, Dieta, CantPersonas, Fecha.Date, Horario, cancellationToken);

            Message = result.Message;
            ReservaId = result.ReservaId;
            Suggestions = result.Suggestions;

            return Page();
        }
    }
}
