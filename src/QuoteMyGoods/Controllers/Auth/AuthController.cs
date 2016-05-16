using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using QuoteMyGoods.Models;
using QuoteMyGoods.ViewModels;
using System.Threading.Tasks;

namespace QuoteMyGoods.Controllers.Auth
{
    [AllowAnonymous]
    public class AuthController:Controller
    {
        private SignInManager<QMGUser> _signInManager;
        private UserManager<QMGUser> _userManager;

        public AuthController(SignInManager<QMGUser> signInManager, UserManager<QMGUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
        public async Task<IActionResult> SignUp(QMGUser user)
        {
            if (await _userManager.FindByEmailAsync(user.Email) == null)
            {
                var newUser = new QMGUser()
                {
                    UserName = "noalgalex",
                    Email = "noalgalex@gmail.com"
                };

                var createdUser = await _userManager.CreateAsync(newUser, user.PasswordHash);
                if (createdUser.Succeeded)
                {
                    return RedirectToAction("Login",new { vm = new LoginViewModel { Username = "", Password = "" }, returnUrl = ""});
                }else
                {
                    return RedirectToAction("Forbidden");
                }
            }
            else
            {
                return RedirectToAction("Login", new { vm = new LoginViewModel { Username = "", Password = "" }, returnUrl = "" });
            }
        }
    }
}
