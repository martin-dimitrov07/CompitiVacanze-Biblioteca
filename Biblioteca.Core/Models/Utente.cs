using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Models
{
    public class Utente
    {
        public int IdUtente { get; set; }

        public DateTime? DataNascita { get; set; }

        public string Nome { get; set; }

        public string Cognome { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

    }
}
