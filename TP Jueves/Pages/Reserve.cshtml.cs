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

        [BindProperty(SupportsGet = true)]
        public int Step { get; set; } = 1;

        public Restaurante? RestauranteSeleccionado { get; set; }
        public ApplicationUser? CurrentUser { get; set; }
        public List<HorarioRestaurante> HorariosDisponibles { get; set; } = new();
        public Dictionary<string, int> DisponibilidadPorHorario { get; set; } = new();

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
        public string HoraSeleccionada { get; set; } = string.Empty;

        [BindProperty]
        [Obsolete("Usar HoraSeleccionada")]
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

            // Recuperar datos de query string si existen
            if (Request.Query.ContainsKey("fecha"))
            {
                if (DateTime.TryParse(Request.Query["fecha"], out var fecha))
                    Fecha = fecha;
            }
            if (Request.Query.ContainsKey("cantPersonas"))
            {
                if (int.TryParse(Request.Query["cantPersonas"], out var cant))
                    CantPersonas = cant;
            }
            if (Request.Query.ContainsKey("dieta"))
            {
                if (int.TryParse(Request.Query["dieta"], out var dieta))
                    Dieta = (Dieta)dieta;
            }

            // Si viene un restauranteId, validar que existe
            if (RestauranteId.HasValue)
            {
                RestauranteSeleccionado = await _db.Restaurantes
                    .Include(r => r.Mesas)
                    .Include(r => r.Horarios)
                    .FirstOrDefaultAsync(r => r.Id == RestauranteId.Value && !r.IsDeleted);
                
                if (RestauranteSeleccionado == null)
                    return RedirectToPage("/Restaurants/Browse");

                // Si estamos en el paso 2, cargar disponibilidad de horarios
                if (Step == 2 && Fecha != default)
                {
                    HorariosDisponibles = RestauranteSeleccionado.Horarios
                        .Where(h => h.EstaActivo)
                        .OrderBy(h => h.Hora)
                        .ToList();

                    // Verificar si hay al menos una mesa que pueda acomodar la cantidad de personas
                    var hayMesaSuficiente = await _db.Mesas
                        .AnyAsync(m => m.RestauranteId == RestauranteId.Value && m.Capacidad >= CantPersonas);

                    if (!hayMesaSuficiente)
                    {
                        Message = $"Lo sentimos, no tenemos mesas disponibles para {CantPersonas} personas. La capacidad m&aacute;xima de nuestras mesas es {await _db.Mesas.Where(m => m.RestauranteId == RestauranteId.Value).MaxAsync(m => (int?)m.Capacidad) ?? 0} personas.";
                        HorariosDisponibles.Clear();
                        return Page();
                    }

                    // Calcular disponibilidad REAL basada en mesas físicas
                    foreach (var horario in HorariosDisponibles)
                    {
                        // Contar mesas disponibles que pueden acomodar la cantidad de personas
                        var mesasDisponibles = await _db.Mesas
                            .Where(m => m.RestauranteId == RestauranteId.Value && m.Capacidad >= CantPersonas)
                            .Where(m => !_db.Reservas.Any(r => 
                                r.MesaId == m.Id &&
                                r.Fecha.Date == Fecha.Date &&
                                r.HoraReserva == horario.Hora &&
                                !r.IsCancelled))
                            .CountAsync();

                        // Si hay al menos una mesa disponible, el horario está disponible
                        DisponibilidadPorHorario[horario.Hora] = mesasDisponibles > 0 ? 1 : 0;
                    }
                }
            }

            ViewData["BodyClass"] = "has-restaurant-bg";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
        {
            return await OnPostStep1Async();
        }

        public async Task<IActionResult> OnPostStep1Async()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
                return NotFound();

            // Validar fecha y cantidad
            if (Fecha.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(Fecha), "La fecha debe ser hoy o una fecha futura.");
                return Page();
            }

            if (CantPersonas < 1 || CantPersonas > 20)
            {
                ModelState.AddModelError(nameof(CantPersonas), "La cantidad debe estar entre 1 y 20 personas.");
                return Page();
            }

            // Redirigir al paso 2 con los datos
            return RedirectToPage(new { 
                restauranteId = RestauranteId, 
                step = 2,
                fecha = Fecha.ToString("yyyy-MM-dd"),
                cantPersonas = CantPersonas,
                dieta = (int)Dieta
            });
        }

        public async Task<IActionResult> OnPostStep2Async(CancellationToken cancellationToken = default)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
                return NotFound();

            if (!RestauranteId.HasValue)
            {
                Message = "Debe seleccionar un restaurante.";
                return RedirectToPage("/Restaurants/Browse");
            }

            // Validar DNI
            var dniNorm = (Dni ?? string.Empty).Trim();
            if (!Regex.IsMatch(dniNorm, "^\\d{8}$"))
            {
                ModelState.AddModelError(nameof(Dni), "DNI inv&aacute;lido.");
                Step = 2;
                await RecargarHorariosDisponibles(cancellationToken);
                return Page();
            }

            if (string.IsNullOrEmpty(HoraSeleccionada))
            {
                Message = "Debe seleccionar un horario.";
                Step = 2;
                await RecargarHorariosDisponibles(cancellationToken);
                return Page();
            }

            // Cargar restaurante
            RestauranteSeleccionado = await _db.Restaurantes
                .Include(r => r.Mesas)
                .Include(r => r.Horarios)
                .FirstOrDefaultAsync(r => r.Id == RestauranteId.Value && !r.IsDeleted, cancellationToken);

            if (RestauranteSeleccionado == null)
            {
                return RedirectToPage("/Restaurants/Browse");
            }

            // Verificar que el horario existe y está activo
            var horarioSeleccionado = RestauranteSeleccionado.Horarios.FirstOrDefault(h => h.Hora == HoraSeleccionada && h.EstaActivo);
            if (horarioSeleccionado == null)
            {
                Message = "Horario no v&aacute;lido o inactivo.";
                Step = 2;
                await RecargarHorariosDisponibles(cancellationToken);
                return Page();
            }

            // Buscar mesa disponible que acomode a la cantidad de personas
            var mesaDisponible = await _db.Mesas
                .Where(m => m.RestauranteId == RestauranteId.Value && 
                           m.Capacidad >= CantPersonas)
                .Where(m => !_db.Reservas.Any(r => 
                    r.MesaId == m.Id &&
                    r.Fecha.Date == Fecha.Date &&
                    r.HoraReserva == HoraSeleccionada &&
                    !r.IsCancelled))
                .OrderBy(m => m.Capacidad) // Asignar la mesa más pequeña que acomode al grupo
                .FirstOrDefaultAsync(cancellationToken);

            if (mesaDisponible == null)
            {
                Message = $"No hay mesas disponibles para {CantPersonas} persona(s) en este horario. Por favor selecciona otro horario.";
                Step = 2;
                await RecargarHorariosDisponibles(cancellationToken);
                return Page();
            }

            // Crear reserva
            var nuevaReserva = new Reserva
            {
                Id = Guid.NewGuid(),
                DniCliente = dniNorm,
                ClienteId = CurrentUser.Id,
                Dieta = Dieta,
                CantPersonas = CantPersonas,
                Fecha = Fecha.Date,
                HoraReserva = HoraSeleccionada,
                RestauranteId = RestauranteId.Value,
                MesaId = mesaDisponible.Id,
                CreatedAt = DateTime.UtcNow,
                IsCancelled = false
            };

            _db.Reservas.Add(nuevaReserva);
            await _db.SaveChangesAsync(cancellationToken);

            ReservaId = nuevaReserva.Id;
            Message = "&iexcl;Reserva confirmada correctamente!";
            IsSuccess = true;
            
            return Page();
        }

        private async Task RecargarHorariosDisponibles(CancellationToken cancellationToken = default)
        {
            if (!RestauranteId.HasValue) return;

            RestauranteSeleccionado = await _db.Restaurantes
                .Include(r => r.Mesas)
                .Include(r => r.Horarios)
                .FirstOrDefaultAsync(r => r.Id == RestauranteId.Value, cancellationToken);

            if (RestauranteSeleccionado == null) return;

            HorariosDisponibles = RestauranteSeleccionado.Horarios
                .Where(h => h.EstaActivo)
                .OrderBy(h => h.Hora)
                .ToList();

            // Calcular disponibilidad real basada en mesas físicas
            foreach (var horario in HorariosDisponibles)
            {
                var capacidadTotal = await _db.Mesas
                    .Where(m => m.RestauranteId == RestauranteId.Value)
                    .Where(m => !_db.Reservas.Any(r => 
                        r.MesaId == m.Id &&
                        r.Fecha.Date == Fecha.Date &&
                        r.HoraReserva == horario.Hora &&
                        !r.IsCancelled))
                    .SumAsync(m => m.Capacidad, cancellationToken);

                DisponibilidadPorHorario[horario.Hora] = capacidadTotal;
            }
        }
    }
}
