using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP_Jueves.Data;
using TP_Jueves.Models;

namespace TP_Jueves.Pages.Admin
{
    public class ResetRolesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _db;

        public ResetRolesModel(UserManager<ApplicationUser> userManager, IWebHostEnvironment env, ApplicationDbContext db)
        {
            _userManager = userManager;
            _env = env;
            _db = db;
        }

        public List<(string Email, string Roles)> Users { get; set; } = new();
        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Solo disponible en modo desarrollo
            if (!_env.IsDevelopment())
                return NotFound();

            var allUsers = await _userManager.Users.ToListAsync();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                Users.Add((user.Email ?? "", string.Join(", ", roles)));
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAssignRoleAsync(string email, string role)
        {
            if (!_env.IsDevelopment())
                return NotFound();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                Message = $"Usuario {email} no encontrado.";
                return RedirectToPage();
            }

            // Remover todos los roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }

            // Asignar nuevo rol
            await _userManager.AddToRoleAsync(user, role);
            Message = $"Rol '{role}' asignado a {email}";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAllRolesAsync(string email)
        {
            if (!_env.IsDevelopment())
                return NotFound();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                Message = $"Usuario {email} no encontrado.";
                return RedirectToPage();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                Message = $"Se removieron todos los roles de {email}";
            }

            return RedirectToPage();
        }
    }
}
