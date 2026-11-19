using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;


namespace TARge25Shop.Models.Account

{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }

        public string? FirstName { get; set; }
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    }
}