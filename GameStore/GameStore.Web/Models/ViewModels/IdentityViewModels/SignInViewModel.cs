using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.ViewModels.IdentityViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "EmailMessage")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordMessage")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}