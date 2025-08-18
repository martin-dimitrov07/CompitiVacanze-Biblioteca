using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Models
{
    public class Libro
    {
        [Key]
        public int IdLibro { get; set; }

        [Required(ErrorMessage = "Inserire il titolo")]
        [StringLength(100)]
        public string Titolo { get; set; }

        [Required(ErrorMessage = "Inserire l'autore")]
        public int IdAutore { get; set; }
        public Autore? Autore { get; set; }

        [Required(ErrorMessage = "Inserire l'anno")]
        public int Anno { get; set; }

        [Required(ErrorMessage = "Inserire il paese")]
        public int IdPaese { get; set; }
        public Nazione? Paese { get; set; }

        [Required(ErrorMessage = "Inserire la lingua")]
        public int IdLingua { get; set; }
        public Lingua? Lingua { get; set; }

        [Required(ErrorMessage = "Inserire il prezzo")]
        public decimal Prezzo { get; set; }

        [Required(ErrorMessage = "Inserire il numero di pagine")]
        public int Pagine { get; set; }

    }
}
