using Biblioteca.Core.Models;
using Biblioteca.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            List<Libro>? libri = _repo.GetLibri("");

            return View(libri);
        }
    }
}
