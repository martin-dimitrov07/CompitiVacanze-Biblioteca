using Biblioteca.Core.Models;
using Biblioteca.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

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
            return View(new Login());
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                List<Utente>? users = _repo.GetUtenti($"Email = '{model.Email}'",
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
                            Console.WriteLine("Login effettuato con successo.");

                            // Crea i claims per l'utente autenticato per il cookie di autenticazione per il login
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.Email),
                                // puoi aggiungere anche: new Claim(ClaimTypes.Role, user.Ruolo)
                            };

                            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                            var principal = new ClaimsPrincipal(identity);

                            await HttpContext.SignInAsync("MyCookieAuth", principal); // crea il cookie

                            if(user.Email == "admin@admin.com" && BCrypt.Net.BCrypt.Verify("Admin", user.PasswordHash))
                            {
                                Console.WriteLine("Accesso come Admin.");
                                return RedirectToAction("IndexAdmin", "Home");
                            }
                            else
                            {
                                Console.WriteLine("Accesso come Utente.");
                                return RedirectToAction("IndexCliente", "Home", new { idCliente = user.IdUtente });
                            }
                        }
                        else
                            ModelState.AddModelError(string.Empty, "Email o Password incorretti");
                    }
                }
                else
                    ModelState.AddModelError(string.Empty, "Email o Password incorretti");

                Console.WriteLine("Email o password non corretti.");
            }

            ViewBag.Title = "Login";
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth"); // cancella il cookie
            return RedirectToAction("Login", "Account");
        }
    }
}
