using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrocusoftLibrary.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Zəhmət olmasa, boşluğu doldurun")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa, boşluğu doldurun"), DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
