using Biblioteca.Core.Models;
using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly Repository _repo;

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login model)
        {
            //Aggiustare

            if(ModelState.IsValid)
            {
                var user = _repo.GetElements("Utenti", "Email = @Email AND Password = @Password",
                    [new SqlParameter("@Email", model.Email),
                    new SqlParameter("@Password", model.Password)]);

                if (user != null)
                {
                    ModelState.AddModelError("", "Login effettuato con successo.");
                }
                else
                {
                    ModelState.AddModelError("", "Email o password non corretti.");
                }
            }

            return View(model);
        }
    }
}
