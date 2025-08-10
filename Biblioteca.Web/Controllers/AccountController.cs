using Biblioteca.Core.Models;
using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly Repository? _repo;

        public AccountController(IConfiguration configuration)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            _repo = new Repository(connStr);
        }

        public IActionResult Login()
        {
            ViewBag.Title = "Login";
            return View(new Biblioteca.Core.Models.Login());
        }

        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                List<Utente>? users = _repo.GetElements<Utente>("Utenti", $"Email = '{model.Email}'",
                    new SqlParameter[] {
                        new SqlParameter("@Email", model.Email),
                        new SqlParameter("@PasswordHash", passwordHash) 
                    });

                if (users != null)
                {
                    foreach(Utente user in users)
                    {
                        if (BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                        {
                            // Login successful
                            // Here you can set session variables or cookies as needed
                            // For example: HttpContext.Session.SetString("UserEmail", user.Email);
                            //return RedirectToAction("Index", "Home");
                            Console.WriteLine("Login effettuato con successo.");
                        }
                    }
                }
                
                Console.WriteLine("Email o password non corretti.");
            }

            return View(model);
        }
    }
}
