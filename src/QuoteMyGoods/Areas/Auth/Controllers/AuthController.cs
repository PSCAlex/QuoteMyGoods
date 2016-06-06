﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using QuoteMyGoods.Areas.Auth.Models;
using System.Threading.Tasks;

namespace QuoteMyGoods.Areas.Auth.Controllers
{
    [Area("Auth")]
    [AllowAnonymous]
    public class AuthController:Controller
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Products", "Products");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel vm, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);

                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Products", "Products");
                    }
                    else
                    {
                        Redirect(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is incorrect");
                }
            }

            return View();
        }

        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "App");
        }

        public IActionResult Unauthorized()
        {
            /*
            const string Issuer = "http://alexlogan.io";
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "Alex", ClaimValueTypes.String, Issuer));
            var userIdentity = new ClaimsIdentity("SuperSecureLogin");
            userIdentity.AddClaims(claims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.Authentication.SignInAsync("Cookie", userPrincipal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                    IsPersistent = false,
                    AllowRefresh = false
                });

    */

            return RedirectToAction("Forbidden","Auth");
        }

        public IActionResult Forbidden()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.FindByEmailAsync(user.Username) == null)
                {
                    var newUser = new IdentityUser()
                    {
                        UserName = user.Username,
                        Email = user.Username
                    };

                    var plebRole = await _roleManager.FindByNameAsync("Pleb");
                    newUser.Roles.Add(new IdentityUserRole<string>()
                    {
                        RoleId = plebRole.Id,
                        UserId = newUser.Id
                    });

                    var createdUser = await _userManager.CreateAsync(newUser, user.Password1);
                    if (createdUser.Succeeded)
                    {
                        var signInResult = await _signInManager.PasswordSignInAsync(user.Username, user.Password1, true, false);

                        if (signInResult.Succeeded)
                        {
                            return RedirectToAction("Products", "Products");
                        }
                        else
                        {
                            return RedirectToAction("Forbidden");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Forbidden");
                    }
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }

            return View();
        }
    }
}
