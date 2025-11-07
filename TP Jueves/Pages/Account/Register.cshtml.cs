using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TP_Jueves.Models;

namespace TP_Jueves.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            [Required]
            [Display(Name = "Nombre")]
            public string Nombre { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Apellido")]
            public string Apellido { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
            [Display(Name = "Confirmar contraseña")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Tipo de Cuenta")]
            public string Rol { get; set; } = "Cliente";
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Validar si ya existe un usuario con ese email
            var existingUser = await _userManager.FindByEmailAsync(Input.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, 
                    $"Ya existe una cuenta registrada con el email {Input.Email}. Por favor usa otro email o inicia sesi&oacute;n.");
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                Nombre = Input.Nombre,
                Apellido = Input.Apellido
            };

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                // Asignar rol
                var roleResult = await _userManager.AddToRoleAsync(user, Input.Rol);
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Index");
            }

            // Traducir errores comunes de Identity al español
            foreach (var error in result.Errors)
            {
                var errorMessage = error.Code switch
                {
                    "DuplicateUserName" => $"El email {Input.Email} ya est&aacute; registrado.",
                    "DuplicateEmail" => $"El email {Input.Email} ya est&aacute; registrado.",
                    "InvalidEmail" => "El formato del email no es v&aacute;lido.",
                    "PasswordTooShort" => "La contrase&ntilde;a debe tener al menos 6 caracteres.",
                    "PasswordRequiresDigit" => "La contrase&ntilde;a debe contener al menos un n&uacute;mero.",
                    "PasswordRequiresLower" => "La contrase&ntilde;a debe contener al menos una letra min&uacute;scula.",
                    "PasswordRequiresUpper" => "La contrase&ntilde;a debe contener al menos una letra may&uacute;scula.",
                    "PasswordRequiresNonAlphanumeric" => "La contrase&ntilde;a debe contener al menos un car&aacute;cter especial.",
                    _ => error.Description
                };
                
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            return Page();
        }
    }
}
