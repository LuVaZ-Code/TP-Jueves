using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using TP_Jueves.Models;
using TP_Jueves.Services;
using TP_Jueves.Data;

namespace TP_Jueves.Pages
{
    /// <summary>
    /// PageModel for reservation form.
    /// Handles GET (form) and POST (attempt to reservar).
    /// Requires authentication as Cliente.
    /// </summary>
    [Authorize(Roles = "Cliente")]
    public class ReserveModel : PageModel
    {
        private readonly RestauranteService _restaurante;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReserveModel(RestauranteService restaurante, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _restaurante = restaurante;
            _db = db;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public int? RestauranteId { get; set; }

        public Restaurante? RestauranteSeleccionado { get; set; }
        public ApplicationUser? CurrentUser { get; set; }
        public List<TurnoDisponible> HorariosDisponibles { get; set; } = new();

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
        public DateTime Fecha { get; set; } = DateTime.Today.AddDays(1);

        [BindProperty]
        public Horario Horario { get; set; } = Horario.H20_22;

        [BindProperty]
        public int? TurnoDisponibleId { get; set; }

        public string? Message { get; set; }
        public Guid? ReservaId { get; set; }
        public List<(DateTime fecha, Horario horario)> Suggestions { get; set; } = new();
        public bool IsSuccess { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
                return NotFound();

            // Si viene un restauranteId, validar que existe
            if (RestauranteId.HasValue)
            {
                RestauranteSeleccionado = await _db.Restaurantes
                    .Include(r => r.Mesas)
                    .FirstOrDefaultAsync(r => r.Id == RestauranteId.Value && !r.IsDeleted);
                
                if (RestauranteSeleccionado == null)
                    return RedirectToPage("/Restaurants/Browse");

                // Cargar horarios disponibles del restaurante para los próximos 30 días
                var today = DateTime.Today;
                var futureDate = today.AddDays(30);
                HorariosDisponibles = await _db.TurnosDisponibles
                    .Where(t => t.RestauranteId == RestauranteId.Value && 
                                t.Fecha >= today && 
                                t.Fecha <= futureDate && 
                                t.IsActive)
                    .OrderBy(t => t.Fecha)
                    .ThenBy(t => t.Horario)
                    .ToListAsync();
            }

            ViewData["BodyClass"] = "has-restaurant-bg";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
                return NotFound();

            // Validar modelo
            if (!ModelState.IsValid)
            {
                if (RestauranteId.HasValue)
                {
                    RestauranteSeleccionado = await _db.Restaurantes
                        .Include(r => r.Mesas)
                        .FirstOrDefaultAsync(r => r.Id == RestauranteId.Value && !r.IsDeleted);
                }
                return Page();
            }

            // Validaciones adicionales
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

            // Si hay restaurante especificado, validar que exista
            if (RestauranteId.HasValue)
            {
                RestauranteSeleccionado = await _db.Restaurantes
                    .Include(r => r.Mesas)
                    .FirstOrDefaultAsync(r => r.Id == RestauranteId.Value && !r.IsDeleted);

                if (RestauranteSeleccionado == null)
                {
                    return RedirectToPage("/Restaurants/Browse");
                }

                // Buscar mesa disponible
                var mesaDisponible = await _restaurante.BuscarMesaAsync(CantPersonas, Fecha.Date, Horario, cancellationToken);
                
                if (mesaDisponible == null)
                {
                    Message = "No hay mesas disponibles para la cantidad de personas en ese turno.";
                    // Buscar sugerencias
                    var allHorarios = Enum.GetValues(typeof(Horario)).Cast<Horario>().ToList();
                    foreach (var altHorario in allHorarios)
                    {
                        if (altHorario == Horario) continue;
                        var altMesa = await _restaurante.BuscarMesaAsync(CantPersonas, Fecha.Date, altHorario, cancellationToken);
                        if (altMesa != null)
                            Suggestions.Add((Fecha.Date, altHorario));
                    }
                    return Page();
                }

                // Crear reserva vinculada a usuario y restaurante
                var reserva = new Reserva
                {
                    Id = Guid.NewGuid(),
                    DniCliente = dniNorm,
                    ClienteId = CurrentUser.Id,
                    Dieta = Dieta,
                    CantPersonas = CantPersonas,
                    Fecha = Fecha.Date,
                    Horario = Horario,
                    RestauranteId = RestauranteId.Value,
                    MesaId = mesaDisponible.Id,
                    TurnoDisponibleId = TurnoDisponibleId,
                    CreatedAt = DateTime.UtcNow,
                    IsCancelled = false
                };

                _db.Reservas.Add(reserva);
                await _db.SaveChangesAsync(cancellationToken);

                ReservaId = reserva.Id;
                Message = "¡Reserva confirmada correctamente!";
                IsSuccess = true;
            }
            else
            {
                Message = "Debe seleccionar un restaurante para hacer la reserva.";
            }

            if (RestauranteId.HasValue)
            {
                RestauranteSeleccionado = await _db.Restaurantes
                    .Include(r => r.Mesas)
                    .FirstOrDefaultAsync(r => r.Id == RestauranteId.Value && !r.IsDeleted);
            }

            return Page();
        }
    }
}
