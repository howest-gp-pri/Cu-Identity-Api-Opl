using System.ComponentModel.DataAnnotations;

namespace Pri.Identity.Api.Dtos
{
    public class RegisterUserRequestDto : LoginUserRequestDto
    {
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string City { get; set; }
    }
}
