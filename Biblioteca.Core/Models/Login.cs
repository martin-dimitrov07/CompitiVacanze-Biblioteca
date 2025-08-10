using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Core.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Inserire l'indirizzo email")]
        [StringLength(80)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Inserire la password")]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
