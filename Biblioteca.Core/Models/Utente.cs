using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Models
{
    public class Utente
    {
        [Key]
        public int IdUtente { get; set; }

        public DateTime? DataNascita { get; set; }

        [Required(ErrorMessage = "Inserire il nome")]
        [StringLength(30)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Inserire il cognome")]
        [StringLength(30)]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "Inserire l'indirizzo mail")]
        [StringLength(80)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Inserire la password")]
        [StringLength(50)]
        public string PasswordHash { get; set; }

    }
}
