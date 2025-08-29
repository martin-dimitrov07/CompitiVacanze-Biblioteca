using Biblioteca.Data;
using Biblioteca.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Biblioteca.Core.Models;

namespace Biblioteca.Web.Controllers
{
    public class DocXController : Controller
    {
        private readonly Repository _repo;
        private readonly WordDocumentService _wordDocumentService;

        public DocXController(IConfiguration configuration)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            _repo = new Repository(connStr);
            _wordDocumentService = new WordDocumentService();
        }

        public IActionResult DownloadWord(int idCliente)
        {
            var prenotazioni = _repo.GetPrenotazioni("IdUtente=@IdUtente", new SqlParameter[] { new SqlParameter("@IdUtente", idCliente) });

            var prestiti = new List<Prestito>();

            foreach (var prenotazione in prenotazioni)
            {
                var prestito = _repo.GetPrestiti("IdPrenotazione=@IdPrenotazione AND DataFine=@Data", new SqlParameter[] { new SqlParameter("@IdPrenotazione", prenotazione.IdPrenotazione), new SqlParameter("@Data", DateTime.Today) });

                if(prestito != null && prestito.Count > 0)
                {
                    prestiti.AddRange(prestito);
                }
            }

            int[] IdPrenots = prestiti.Select(p => p.IdPrenotazione).ToArray();

            var libri = new List<Libro>();

            foreach (var prenotazione in prenotazioni)
            {
                if (IdPrenots.Contains(prenotazione.IdPrenotazione))
                {
                    libri.AddRange(_repo.GetLibri("IdLibro=@IdLibro", new SqlParameter[] { new SqlParameter("@IdLibro", prenotazione.IdLibro) }));
                }
            }
            
            var autori = new List<Autore>();

            foreach (var libro in libri)
            {
                autori.AddRange(_repo.GetAutori("IdAutore=@IdAutore", new SqlParameter[] { new SqlParameter("@IdAutore", libro.IdAutore) }));
            }

            var nazioni = new List<Nazione>();

            foreach (var libro in libri)
            {
                nazioni.AddRange(_repo.GetNazioni("IdPaese=@IdPaese", new SqlParameter[] { new SqlParameter("@IdPaese", libro.IdPaese) }));
            }

            var lingue = new List<Lingua>();

            foreach (var libro in libri)
            {
                lingue.AddRange(_repo.GetLingue("IdLingua=@IdLingua", new SqlParameter[] { new SqlParameter("@IdLingua", libro.IdLingua) }));
            }

            var utente = _repo.GetUtenti("IdUtente=@IdUtente", new SqlParameter[] { new SqlParameter("@IdUtente", idCliente) })[0];

            var fileContents = _wordDocumentService.GenerateReminderLetter(libri, autori, nazioni, lingue, utente);

            return File(
                fileContents,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                $"Biblioteca_Report_{DateTime.Now:yyyyMMdd_HHmmss}.docx");
        }
    }
}
