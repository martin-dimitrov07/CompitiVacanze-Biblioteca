using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Models
{
    public class Prenotazione
    {
        [Key]
        public int IdPrenotazione { get; set; }

        public int IdLibro { get; set; }
        public Libro Libro { get; set; }

        public int IdUtente { get; set; }
        public Utente Utente { get; set; }

    }
}
