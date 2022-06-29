using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LiquefiedPetroleumGas.Models
{
    public class RoleEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<Users> Members { get; set; }
        public IEnumerable<Users> NonMembers { get; set; }
        public string RoleName { get; set; }
        public string[] AddIds { get; set; }
        public string[] DeleteIds { get; set; }
    }
}
