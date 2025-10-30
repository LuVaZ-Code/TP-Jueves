using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public string Dni { get; set; } = string.Empty;

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
            // No-op
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(Dni))
            {
                ModelState.AddModelError(nameof(Dni), "DNI es requerido.");
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
