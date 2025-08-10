using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult IndexAdmin()
        {
            ViewBag.Title = "Home Page";
            ViewBag.Message = "Lieto di salutarla, Admin!";
            ViewBag.Utente = "Admin";
            return View("Index");
        }

        [Authorize]
        public IActionResult IndexCliente()
        {
            ViewBag.Title = "Home Page";
            ViewBag.Message = "Benvenuto nella Biblioteca Digitale!";
            ViewBag.Utente = "Cliente";
            return View("Index");
        }
    }
}
