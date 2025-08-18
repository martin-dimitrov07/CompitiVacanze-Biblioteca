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
    public class LibroController : Controller
    {
        private readonly Repository? _repo;

        public LibroController(IConfiguration configuration)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            _repo = new Repository(connStr);
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Title = "Libri";
            ViewBag.Utente = "Admin";

            List<Libro>? libri = _repo.GetLibri("");

            return View(libri);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Aggiungi Libro";
            ViewBag.Utente = "Admin";

            ViewBag.Autori = new SelectList(_repo.GetAutori(""), "IdAutore", "Nominativo");
            ViewBag.Paesi = new SelectList(_repo.GetNazioni(""), "IdPaese", "Nome");
            ViewBag.Lingue = new SelectList(_repo.GetLingue(""), "IdLingua", "Nome");

            return View(new Libro());
        }

        [HttpPost]
        public IActionResult Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                _repo.InsertElement("Libri", libro, new SqlParameter[] {
                    new SqlParameter("@Titolo", libro.Titolo)
                });

                ViewBag.Utente = "Admin";
                return RedirectToAction("Index");
            }

            ViewBag.Title = "Aggiungi Libro";
            ViewBag.Utente = "Admin";
            ViewBag.Autori = new SelectList(_repo.GetAutori(""), "IdAutore", "Nominativo");
            ViewBag.Paesi = new SelectList(_repo.GetNazioni(""), "IdPaese", "Nome");
            ViewBag.Lingue = new SelectList(_repo.GetLingue(""), "IdLingua", "Nome");
            return View(libro);
        }
    }
}
