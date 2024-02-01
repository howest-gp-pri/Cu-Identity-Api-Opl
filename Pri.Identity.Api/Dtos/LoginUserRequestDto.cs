using System.ComponentModel.DataAnnotations;

namespace Pri.Identity.Api.Dtos
{
    public class LoginUserRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
