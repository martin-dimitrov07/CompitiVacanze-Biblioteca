using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Web.ViewModels
{
    public class PrenotazionePrestito
    {
        // campi per Prenotazione
        [Required(ErrorMessage = "Inserire l'autore")]
        public int IdLibro { get; set; }

        // campi per Prestito
        [Required(ErrorMessage = "Inserire la data d'inizio del prestito")]

        public DateTime DataInizioPrestito { get; set; }

        [Required(ErrorMessage = "Inserire la data di fine del prestito")]
        public DateTime? DataFinePrestito { get; set; }
    }

}
