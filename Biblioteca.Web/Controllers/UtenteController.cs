using Biblioteca.Core.Models;
using Biblioteca.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace Biblioteca.Web.Controllers
{
    public class UtenteController : Controller
    {
        private readonly Repository? _repo;

        public UtenteController(IConfiguration configuration)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            _repo = new Repository(connStr);
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Title = "Utenti";
            ViewBag.Utente = "Admin";

            List<Utente>? utenti = _repo.GetUtenti("");

            return View(utenti);
        }

        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Title = "Aggiungi Utente";
            ViewBag.Utente = "Admin";

            return View(new Utente());
        }

        [HttpPost]
        public IActionResult Create(Utente utente)
        {
            if (ModelState.IsValid)
            {
                utente.PasswordHash = BCrypt.Net.BCrypt.HashPassword(utente.PasswordHash);

                _repo.InsertElement("Utenti", utente);

                ViewBag.Utente = "Admin";
                return RedirectToAction("Index");
            }
            return View(utente);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Modifica Utente";
            ViewBag.Utente = "Admin";
            List<Utente>? utente = _repo.GetUtenti($"IdUtente=@IdUtente", new SqlParameter[] { new SqlParameter("@IdUtente", id) });

            return View(utente[0]);
        }

        [HttpPost]
        public IActionResult Edit(Utente utente)
        {
            if (ModelState.IsValid)
            {
                if (utente.PasswordHash != "nocambiata")
                {
                    utente.PasswordHash = BCrypt.Net.BCrypt.HashPassword(utente.PasswordHash);
                }
                else
                {
                    List<Utente> existingUtente = _repo.GetUtenti($"IdUtente=@IdUtente", new SqlParameter[] { new SqlParameter("@IdUtente", utente.IdUtente) });
                    if (existingUtente != null)
                    {
                        utente.PasswordHash = existingUtente[0].PasswordHash;
                    }
                }

                _repo.UpdateElement("Utenti", utente, "IdUtente=@IdUtente", [new SqlParameter("@IdUtente", utente.IdUtente)]);
                ViewBag.Utente = "Admin";
                return RedirectToAction("Index");
            }
            return View(utente);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _repo.DeleteElement("Utenti", $"IdUtente=@IdUtente", new SqlParameter[] {
                new SqlParameter("@IdUtente", id)
            });
            ViewBag.Utente = "Admin";
            return RedirectToAction("Index");
        }
    }
}
