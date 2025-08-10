using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Models
{
    public class Prestito
    {
        public int IdPrestito { get; set; }

        public int IdLibro { get; set; }
        public Libro Libro { get; set; }

        public int IdUtente { get; set; }
        public Utente Utente { get; set; }

        public DateTime DataInizio { get; set; }

    }
}
