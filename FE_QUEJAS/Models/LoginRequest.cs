using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FE_QUEJAS.Models
{
    
    public class LoginRequest
    {
        [DisplayName("Nombre de usuario")]
        [Required(ErrorMessage = "El campo usuario es obligatorio")]
        public string Usuario { get; set; }
        [DisplayName("Contraseña")]        
        [DataType(DataType.Password)]
        public string Pass { get; set; }
        // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$", ErrorMessage = "La contraseña no es segura")]
    }


    
}