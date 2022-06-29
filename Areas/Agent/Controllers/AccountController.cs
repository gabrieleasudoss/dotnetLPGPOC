using LiquefiedPetroleumGas.Infrastructure;
using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LiquefiedPetroleumGas.Areas.Agent.Controllers
{
    [Area("Agent")]
    public class AccountController : Controller
    {
        private readonly LiquefiedPetroleumGasLoginAndRegistrationContext context;
        public AccountController(LiquefiedPetroleumGasLoginAndRegistrationContext context)
        {
            this.context = context;
        }

        //private readonly UserManager<Agents> userManager;
        //private readonly SignInManager<Agents> signInManager;
        //private IPasswordHasher<Agents> passwordHasher;
        //private readonly RoleManager<Agents> roleManager;
        //private readonly IUserClaimsPrincipalFactory<Agents> userClaimsPrincipalFactory;

        //public AccountController(UserManager<Agents> userManager,
        //                        SignInManager<Agents> signInManager,
        //                        IPasswordHasher<Agents> passwordHasher,
        //                        RoleManager<Agents> roleManager,
        //                        IUserClaimsPrincipalFactory<Agents> userClaimsPrincipalFactory)
        //{
        //    this.userManager = userManager;
        //    this.signInManager = signInManager;
        //    this.passwordHasher = passwordHasher;
        //    this.roleManager = roleManager;
        //    this.userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        //}

        // GET /agent/account/register
        public IActionResult Register() => View("~/Views/Account/Register.cshtml");

        // POST /agent/account/register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                Agents agents = new Agents
                {
                    Id = "",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = Guid.NewGuid().ToString(),
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = Guid.NewGuid().ToString(),
                    UserName = customer.UserName,
                    PasswordHash = customer.PasswordHash,
                    PhoneNumber = customer.FirstName,
                    Email = customer.EmailId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    //LastName = customer.AadharId,
                    Address = customer.Address,
                    ZipCode = customer.ZipCode
                };

                context.Agents.Add(agents);

                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(Customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Agents agents = new Agents
        //        {
        //            Id = "",
        //            CreatedAt = DateTime.UtcNow,
        //            CreatedBy = Guid.NewGuid().ToString(),
        //            UpdatedAt = DateTime.UtcNow,
        //            UpdatedBy = Guid.NewGuid().ToString(),
        //            UserName = customer.UserName,
        //            PasswordHash = customer.PasswordHash,
        //            PhoneNumber = customer.FirstName,
        //            Email = customer.EmailId,
        //            FirstName = customer.FirstName,
        //            LastName = customer.LastName,
        //            //LastName = customer.AadharId,
        //            Address = customer.Address,
        //            ZipCode = customer.ZipCode
        //        };

        //        IdentityResult result = await userManager.CreateAsync(agents, customer.PasswordHash);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Login");
        //        }
        //        else
        //        {
        //            foreach (IdentityError error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error.Description);
        //            }
        //        }
        //    }

        //    return View(customer);
        //}

        //GET /account/login

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login
            {
                ReturnUrl = returnUrl
            };

            return View("~/Views/Account/Login.cshtml");
        }

        // POST /account/login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(Login login)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Agents agents = await context.Agents.FindAsync(login.UserName);
        //        if (agents != null)
        //        {
        //            Microsoft.AspNetCore.Identity.SignInResult result = await context.A.PasswordSignInAsync(agents, login.PasswordHash, false, false);
        //            if (result.Succeeded)
        //                return Redirect(login.ReturnUrl ?? "/");
        //        }
        //        ModelState.AddModelError("", "Login failed, wrong credentials.");
        //    }
        //    return View(login);
        //}

        //// GET /account/logout
        //public async Task<IActionResult> Logout()
        //{
        //    await signInManager.SignOutAsync();

        //    return Redirect("/");
        //}

        //// GET /account/edit
        //public async Task<IActionResult> Edit()
        //{
        //    Agents agents = await userManager.FindByNameAsync(User.Identity.Name);
        //    UserEdit agent = new UserEdit(agents);

        //    return View(agent);
        //}

        //// POST /account/edit
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(UserEdit userEdit)
        //{
        //    Agents agents = await userManager.FindByNameAsync(User.Identity.Name);

        //    if (ModelState.IsValid)
        //    {
        //        agents.Email = userEdit.UserName;
        //        agents.Email = userEdit.EmailId;
        //        agents.Email = userEdit.PasswordHash;
        //        agents.Email = userEdit.Address;
        //        agents.Email = userEdit.ZipCode;
        //        if (userEdit.PasswordHash != null)
        //        {
        //            agents.PasswordHash = passwordHasher.HashPassword(agents, userEdit.PasswordHash);
        //        }

        //        IdentityResult result = await userManager.UpdateAsync(agents);
        //        if (result.Succeeded)
        //            TempData["Success"] = "Your information has been edited!";
        //    }

        //    return View();
        //}
    }
}
