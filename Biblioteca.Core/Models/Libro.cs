using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Models
{
    public class Libro
    {
        public int IdLibro { get; set; }

        public string Titolo { get; set; }

        public int IdAutore { get; set; }
        public Autore Autore { get; set; }

        public int Anno { get; set; }

        public int IdPaese { get; set; }
        public Nazione Paese { get; set; }

        public int IdLingua { get; set; }
        public Lingua Lingua { get; set; }

        public decimal Prezzo { get; set; }

        public int Pagine { get; set; }

    }
}
