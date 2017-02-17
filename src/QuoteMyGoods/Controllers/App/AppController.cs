using Microsoft.AspNetCore.Mvc;
using QuoteMyGoods.Services;
using QuoteMyGoods.ViewModels;
using QuoteMyGoods.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace QuoteMyGoods.Controllers.App
{
    [Area("App")]
    public class AppController : Controller
    {
        private IMailService _mailService;

        public AppController(IMailService service)
        {
            _mailService = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = Startup.Configuration["AppSettings:SiteEmailAddress"];

                if (string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("", "Could not send email, configuration problem.");
                }

                if (_mailService.SendMail(email,
                  email,
                  $"Contact Page from {model.Name} ({model.Email})",
                  model.Message))
                {
                    ModelState.Clear();

                    ViewBag.Message = "Mail Sent. Thanks!";

                }
            }

            return View();
        }

        public IActionResult JoinTheDots()
        {
            return View();
        }

        public IActionResult JoinTheDotsTypeScript()
        {
            return View();
        }

        public IActionResult JsPlayground()
        {
            return View();
        }

        public IActionResult MegaForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MegaForm(MegaFormModel megaFormModel)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                ViewBag.Message = "Done!";
            }
            return View();
        }
    }
}
