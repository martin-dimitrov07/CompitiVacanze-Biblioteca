using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Models
{
    public class Autore
    {
        public int IdAutore { get; set; }

        public string Nome { get; set; }

        public string Cognome { get; set; }

        public string Nominativo
        {
            get
            {
                return $"{Nome} {Cognome}";
            }
        }
    }
}
