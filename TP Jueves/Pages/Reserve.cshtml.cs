using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TP_Jueves.Models;
using TP_Jueves.Services;

namespace TP_Jueves.Pages
{
    /// <summary>
    /// PageModel for reservation form.
    /// Handles GET (form) and POST (attempt to reservar).
    /// Requires authentication.
    /// </summary>
    [Authorize]
    public class ReserveModel : PageModel
    {
        private readonly RestauranteService _restaurante;

        public ReserveModel(RestauranteService restaurante)
        {
            _restaurante = restaurante;
        }

        [BindProperty]
        [Required(ErrorMessage = "DNI es requerido.")]
        [RegularExpression("^\\d{8}$", ErrorMessage = "DNI debe contener exactamente 8 dígitos.")]
        public string Dni { get; set; } = string.Empty;

        [BindProperty]
        public Dieta Dieta { get; set; } = Dieta.Normal;

        [BindProperty]
        [Range(1, 20, ErrorMessage = "Debe indicar al menos 1 comensal.")]
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
            // Let model binding run DataAnnotations validators first
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Additional server-side guards (defense in depth)
            var dniNorm = (Dni ?? string.Empty).Trim();
            if (!Regex.IsMatch(dniNorm, "^\\d{8}$"))
            {
                ModelState.AddModelError(nameof(Dni), "DNI inválido.");
                return Page();
            }

            var today = DateTime.Today;
            if (Fecha.Date < today)
            {
                ModelState.AddModelError(nameof(Fecha), "La fecha debe ser hoy o una fecha futura.");
                return Page();
            }

            // Call service with validated dniNorm
            var result = await _restaurante.ReservarAsync(dniNorm, Dieta, CantPersonas, Fecha.Date, Horario, cancellationToken);

            Message = result.Message;
            ReservaId = result.ReservaId;
            Suggestions = result.Suggestions;

            return Page();
        }
    }
}
