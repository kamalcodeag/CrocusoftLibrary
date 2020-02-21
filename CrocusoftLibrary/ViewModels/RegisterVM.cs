using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrocusoftLibrary.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Zəhmət olmasa, boşluğu doldurun"), StringLength(50, ErrorMessage = "Yazının uzunluğu 50 simvoldan uzun ola bilməz")]
        public string FirstName { get; set; }
        [StringLength(50, ErrorMessage = "Yazının uzunluğu 50 simvoldan uzun ola bilməz")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa, boşluğu doldurun"), Range(18, 65, ErrorMessage = "Yaş məhdudiyyəti: 18-65")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa, boşluğu doldurun")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa, boşluğu doldurun"), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa, boşluğu doldurun"), DataType(DataType.Password), Compare("Password", ErrorMessage = "Daxil etdiyiniz şifrə ilə uyğun gəlmir")]
        public string ConfirmPassword { get; set; }
    }
}
