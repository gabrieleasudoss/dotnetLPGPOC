using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiquefiedPetroleumGas.Areas.Agency.Controllers
{
    [Authorize(Roles = "Agency")]
    [Area("Agency")]
    public class UsersController : Controller
    {
        private readonly UserManager<Users> userManager;

        public UsersController(UserManager<Users> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }
    }
}