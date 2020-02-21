using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrocusoftLibrary.ViewModels
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "Zəhmət olmasa, boşluğu doldurun"), DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa, boşluğu doldurun"), DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa, boşluğu doldurun"), DataType(DataType.Password), Compare("NewPassword", ErrorMessage = "Daxil etdiyiniz şifrə ilə uyğun gəlmir")]
        public string ConfirmPassword { get; set; }
    }
}
