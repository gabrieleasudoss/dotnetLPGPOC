using System.ComponentModel.DataAnnotations;

namespace LiquefiedPetroleumGas.Models
{
    public class UserEdit
    {
        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [DataType(DataType.Password), MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string PasswordHash { get; set; }

        [Required, EmailAddress]
        [Display(Name = "Email-ID")]
        public string EmailId { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [Display(Name = "ZipCode")]
        public string ZipCode { get; set; }

        public UserEdit() { }

        public UserEdit(Users appUser)
        {
            UserName = appUser.UserName;
            PasswordHash = appUser.PasswordHash;
            EmailId = appUser.Email;
            Address = appUser.Address;
            ZipCode = appUser.ZipCode;
        }

        //public UserEdit(Agents agents)
        //{
        //    UserName = agents.UserName;
        //    PasswordHash = agents.PasswordHash;
        //    EmailId = agents.Email;
        //    Address = agents.Address;
        //    ZipCode = agents.ZipCode;
        //}
    }
}
