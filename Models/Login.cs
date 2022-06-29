using System.ComponentModel.DataAnnotations;

namespace LiquefiedPetroleumGas.Models
{
    public class Login
    {
        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }
        public string ReturnUrl { get; set; }

    }
}
