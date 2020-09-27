using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RourieWebAPI.Models
{
    public class LoginViewModel
    {
        [StringLength(60), MinLength(1)]
        [Required]
        public string UserName { get; set; }

        [MinLength(1)]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
