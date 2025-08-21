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
        [HttpGet]
        public IActionResult IndexAdmin()
        {
            ViewBag.Title = "Libri";
            ViewBag.Utente = "Admin";
            
            List<Libro>? libri = _repo.GetLibri("");

            return View("Index", libri);
        }

        [Authorize]
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
                _repo.InsertElement("Libri", libro);

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

        [Authorize]
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Modifica Libro";
            ViewBag.Utente = "Admin";
            List<Libro>? libro = _repo.GetLibri($"IdLibro=@IdLibro", new SqlParameter[] { new SqlParameter("@IdLibro", id) });

            ViewBag.Autori = new SelectList(_repo.GetAutori(""), "IdAutore", "Nominativo", libro[0].IdAutore);
            ViewBag.Paesi = new SelectList(_repo.GetNazioni(""), "IdPaese", "Nome", libro[0].IdPaese);
            ViewBag.Lingue = new SelectList(_repo.GetLingue(""), "IdLingua", "Nome", libro[0].IdLingua);
            return View(libro[0]);
        }

        [HttpPost]
        public IActionResult Edit(Libro libro)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateElement("Libri", libro, $"IdLibro=@IdLibro", [new SqlParameter("@IdLibro", libro.IdLibro)]);
                ViewBag.Utente = "Admin";
                return RedirectToAction("Index");
            }

            ViewBag.Title = "Modifica Libro";
            ViewBag.Utente = "Admin";
            ViewBag.Autori = new SelectList(_repo.GetAutori(""), "IdAutore", "Nominativo", libro.IdAutore);
            ViewBag.Paesi = new SelectList(_repo.GetNazioni(""), "IdPaese", "Nome", libro.IdPaese);
            ViewBag.Lingue = new SelectList(_repo.GetLingue(""), "IdLingua", "Nome", libro.IdLingua);
            return View(libro);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _repo.DeleteElement("Libri", $"IdLibro=@IdLibro", new SqlParameter[] {
                new SqlParameter("@IdLibro", id)
            });
            ViewBag.Utente = "Admin";
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult IndexCliente(int idCliente)
        {
            ViewBag.Title = "Prenota Libro";
            ViewBag.Utente = "Cliente";

            List<Libro>? libri = new List<Libro>();

            List<Prenotazione> prenotazioni = _repo.GetPrenotazioni($"IdUtente=@IdUtente", new SqlParameter[] { new SqlParameter("@IdUtente", idCliente) });

            foreach(var prenotazione in prenotazioni)
            {
                libri.AddRange(_repo.GetLibri($"IdLibro=@IdLibro", new SqlParameter[] { new SqlParameter("@IdLibro", prenotazione.IdLibro) }));
            }
            
            ViewBag.IdCliente = idCliente;
            return View("Index", libri);
        }
    }
}
