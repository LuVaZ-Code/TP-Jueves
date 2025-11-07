using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP_Jueves.Data;
using TP_Jueves.Models;

namespace TP_Jueves.Pages.Restaurants.Setup
{
    [Authorize(Roles = "Restaurantero")]
    public class WizardModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public WizardModel(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Step { get; set; } = 1;

        public Restaurante? Restaurante { get; set; }

        [BindProperty]
        public RestauranteInput Input { get; set; } = new();

        [BindProperty]
        public List<MesaInput> Mesas { get; set; } = new();

        [BindProperty]
        public List<TurnoInput> Turnos { get; set; } = new();

        [BindProperty]
        public List<HorarioInput> Horarios { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Si no hay ID, redirigir a crear nuevo restaurante
            if (!Id.HasValue)
            {
                return RedirectToPage("/Restaurants/Create");
            }

            // Cargar restaurante
            Restaurante = await _db.Restaurantes
                .Include(r => r.Mesas)
                .FirstOrDefaultAsync(r => r.Id == Id.Value);

            if (Restaurante == null)
                return NotFound();

            // Verificar propiedad
            var user = await _userManager.GetUserAsync(User);
            if (Restaurante.PropietarioId != user?.Id)
                return Forbid();

            // Si ya está configurado, redirigir al dashboard
            if (Restaurante.ConfiguracionCompletada && Step == 1)
            {
                return RedirectToPage("/Restaurants/Dashboard", new { id = Id });
            }

            // Cargar datos según el paso
            if (Step >= 1)
            {
                Input = new RestauranteInput
                {
                    Nombre = Restaurante.Nombre,
                    Descripcion = Restaurante.Descripcion,
                    Direccion = Restaurante.Direccion,
                    Telefono = Restaurante.Telefono,
                    Email = Restaurante.Email
                };
            }

            if (Step >= 2 && Restaurante.Mesas.Any())
            {
                Mesas = Restaurante.Mesas.Select(m => new MesaInput
                {
                    Capacidad = m.Capacidad,
                    Cantidad = 1
                }).ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostStep1Async()
        {
            if (!Id.HasValue)
                return BadRequest();

            Restaurante = await _db.Restaurantes.FindAsync(Id.Value);
            if (Restaurante == null)
                return NotFound();

            // Actualizar información básica
            Restaurante.Nombre = Input.Nombre;
            Restaurante.Descripcion = Input.Descripcion;
            Restaurante.Direccion = Input.Direccion;
            Restaurante.Telefono = Input.Telefono;
            Restaurante.Email = Input.Email;

            await _db.SaveChangesAsync();

            return RedirectToPage(new { id = Id, step = 2 });
        }

        public async Task<IActionResult> OnPostStep2Async()
        {
            if (!Id.HasValue)
                return BadRequest();

            Restaurante = await _db.Restaurantes
                .Include(r => r.Mesas)
                .FirstOrDefaultAsync(r => r.Id == Id.Value);

            if (Restaurante == null)
                return NotFound();

            // Limpiar mesas existentes
            _db.Mesas.RemoveRange(Restaurante.Mesas);

            // Crear nuevas mesas
            foreach (var mesaInput in Mesas.Where(m => m.Cantidad > 0))
            {
                for (int i = 0; i < mesaInput.Cantidad; i++)
                {
                    Restaurante.Mesas.Add(new Mesa
                    {
                        Capacidad = mesaInput.Capacidad,
                        RestauranteId = Restaurante.Id
                    });
                }
            }

            await _db.SaveChangesAsync();

            return RedirectToPage(new { id = Id, step = 3 });
        }

        public async Task<IActionResult> OnPostStep3Async()
        {
            if (!Id.HasValue)
                return BadRequest();

            Restaurante = await _db.Restaurantes
                .Include(r => r.Horarios)
                .FirstOrDefaultAsync(r => r.Id == Id.Value);

            if (Restaurante == null)
                return NotFound();

            // Limpiar horarios existentes
            _db.HorariosRestaurante.RemoveRange(Restaurante.Horarios);

            // Crear nuevos horarios
            foreach (var horarioInput in Horarios.Where(h => h.Activo && !string.IsNullOrEmpty(h.Hora)))
            {
                Restaurante.Horarios.Add(new HorarioRestaurante
                {
                    RestauranteId = Restaurante.Id,
                    Hora = horarioInput.Hora,
                    Descripcion = horarioInput.Descripcion,
                    EstaActivo = true
                });
            }

            await _db.SaveChangesAsync();

            return RedirectToPage(new { id = Id, step = 4 });
        }

        public async Task<IActionResult> OnPostCompleteAsync()
        {
            if (!Id.HasValue)
                return BadRequest();

            Restaurante = await _db.Restaurantes
                .Include(r => r.Mesas)
                .FirstOrDefaultAsync(r => r.Id == Id.Value);

            if (Restaurante == null)
                return NotFound();

            // Marcar como configurado y activo
            Restaurante.ConfiguracionCompletada = true;
            Restaurante.Estado = EstadoRestaurante.Activo;

            await _db.SaveChangesAsync();

            return RedirectToPage("/Restaurants/Dashboard", new { id = Id });
        }

        public class RestauranteInput
        {
            public string Nombre { get; set; } = string.Empty;
            public string Descripcion { get; set; } = string.Empty;
            public string Direccion { get; set; } = string.Empty;
            public string Telefono { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }

        public class MesaInput
        {
            public int Capacidad { get; set; }
            public int Cantidad { get; set; }
        }

        public class TurnoInput
        {
            public Horario Horario { get; set; }
            public int CapacidadMaxima { get; set; } = 50;
            public bool Activo { get; set; }
        }

        public class HorarioInput
        {
            public string Hora { get; set; } = string.Empty;
            public string? Descripcion { get; set; }
            public bool Activo { get; set; }
        }
    }
}
