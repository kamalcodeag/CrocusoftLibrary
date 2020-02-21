using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrocusoftLibrary.Models
{
    public class CustomUser : IdentityUser
    {
        [Required(ErrorMessage = "Zəhmət olmasa, boşluğu doldurun"), StringLength(50, ErrorMessage = "Yazının uzunluğu 50 simvoldan uzun ola bilməz")]
        public string FirstName { get; set; }
        [StringLength(50, ErrorMessage = "Yazının uzunluğu 50 simvoldan uzun ola bilməz")]
        public string LastName { get; set; }
        [Range(18, 65, ErrorMessage = "Yaş məhdudiyyəti: 18-65")]
        public int Age { get; set; }
    }
}
