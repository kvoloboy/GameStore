using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.ViewModels.IdentityViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "EmailMessage")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordMessage")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "DifferentPasswords")]
        public string ConfirmedPassword { get; set; }
    }
}