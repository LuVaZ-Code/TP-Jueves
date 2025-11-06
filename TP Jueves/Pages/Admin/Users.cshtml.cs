using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP_Jueves.Data;
using TP_Jueves.Models;

namespace TP_Jueves.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public UsersModel(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public class UserWithRoles
        {
            public ApplicationUser User { get; set; }
            public List<string> Roles { get; set; } = new();
        }

        public List<UserWithRoles> Users { get; set; } = new();
        public string? Message { get; set; }

        public async Task OnGetAsync()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            
            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                Users.Add(new UserWithRoles
                {
                    User = user,
                    Roles = roles.ToList()
                });
            }
        }

        public async Task<IActionResult> OnPostAssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            // Remover todos los roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Asignar nuevo rol
            await _userManager.AddToRoleAsync(user, role);
            Message = $"Rol asignado correctamente a {user.Email}";

            return RedirectToPage();
        }
    }
}
