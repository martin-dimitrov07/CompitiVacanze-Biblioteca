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

        public int IdPrenotazione { get; set; }
        public Prenotazione? Prenotazione { get; set; }

        public DateTime DataInizio { get; set; }

        public DateTime DataFine { get; set; }

    }
}
