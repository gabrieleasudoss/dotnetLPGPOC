using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LiquefiedPetroleumGas.Models
{
    public class Customer
    {
        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }
        [Required, EmailAddress]
        [Display(Name = "Email-ID")]
        public string EmailId { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, MinLength(12, ErrorMessage = "Minimum length is 12")]
        [Display(Name = "Aadhar-Id")]
        public string AadharId { get; set; }
        [Required, MinLength(10, ErrorMessage = "Minimum length is 10")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required, MinLength(10, ErrorMessage = "Minimum length is 10")]
        [Display(Name = "Customer Address")]
        public string Address { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [Display(Name = "zipCode")]
        public string ZipCode { get; set; }

        public Agencies Agencies { get; set; }
        public Agents Agents { get; set; }
    }
}
