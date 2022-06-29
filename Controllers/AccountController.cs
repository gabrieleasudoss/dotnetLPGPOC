using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInManager;
        private IPasswordHasher<Users> passwordHasher;

        public AccountController(UserManager<Users> userManager,
                                SignInManager<Users> signInManager,
                                IPasswordHasher<Users> passwordHasher)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;
        }

        //GET /account/register
        [AllowAnonymous]
        public IActionResult Register() => View();

        // POST /account/register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                Users users = new Users
                {
                    UserName = customer.UserName,
                    PasswordHash = customer.PasswordHash,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    AadharId = customer.AadharId,
                    PhoneNumber = customer.PhoneNumber,
                    Email = customer.EmailId,
                    Address = customer.Address,
                    ZipCode = customer.ZipCode
            };
                Trace(users);
                IdentityResult result = await userManager.CreateAsync(users, customer.PasswordHash);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(customer);
        }

        //GET /account/login
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login
            {
                ReturnUrl = returnUrl
            };

            return View(login);
        }

        // POST /account/login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                Users users = await userManager.FindByNameAsync(login.UserName);
                if (users != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(users, login.PasswordHash, false, false);
                    if (result.Succeeded)
                        return Redirect(login.ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Login failed, wrong credentials.");
            }
            return View(login);
        }

        // GET /account/logout
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return Redirect("/");
        }

        // GET /account/edit
        public async Task<IActionResult> Edit()
        {
            Users users = await userManager.FindByNameAsync(User.Identity.Name);
            UserEdit customer = new UserEdit(users);

            return View(customer);
        }

        // POST /account/edit
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEdit userEdit)
        {
            Users users = await userManager.FindByNameAsync(User.Identity.Name);
            Trace(users);
            if (ModelState.IsValid)
            {
                users.Email = userEdit.UserName;
                users.Email = userEdit.EmailId;
                users.Email = userEdit.PasswordHash;
                users.Email = userEdit.Address;
                users.Email = userEdit.ZipCode;
                if (userEdit.PasswordHash != null)
                {
                    users.PasswordHash = passwordHasher.HashPassword(users, userEdit.PasswordHash);
                }

                IdentityResult result = await userManager.UpdateAsync(users);
                if (result.Succeeded)
                    TempData["Success"] = "Your information has been edited!";
            }

            return View();
        }
        Users Trace(Users users)
        {
            users.CreatedAt = DateTime.UtcNow;
            users.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            users.UpdatedAt = DateTime.UtcNow;
            users.UpdatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return users;
        }

    }
}
