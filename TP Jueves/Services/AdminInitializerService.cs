using Microsoft.AspNetCore.Identity;
using TP_Jueves.Data;
using TP_Jueves.Models;

namespace TP_Jueves.Services
{
    /// <summary>
    /// Service to initialize admin user and roles.
    /// </summary>
    public class AdminInitializerService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminInitializerService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Assigns Admin role to a specific user if it exists and doesn't already have it.
        /// </summary>
        public async Task AssignAdminAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return;

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

        /// <summary>
        /// Gets all users without any role assigned.
        /// </summary>
        public async Task<List<ApplicationUser>> GetUsersWithoutRoleAsync()
        {
            var allUsers = _userManager.Users.ToList();
            var usersWithoutRole = new List<ApplicationUser>();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Any())
                {
                    usersWithoutRole.Add(user);
                }
            }

            return usersWithoutRole;
        }
    }
}
