using FE_QUEJAS.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FE_QUEJAS.Models
{
    public class RegistroRequest: IValidatableObject
    {
        [DisplayName("Nombre de usuario")]
        [Required(ErrorMessage = "El campo usuario es obligatorio")]
        public string Usuario { get; set; }
        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }        
        [DataType(DataType.Password)]        
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [DisplayName("Confirmar Contraseña")]
        [Required]
        public string ConfirmPassword { get; set; }
        [DisplayName("Nombre")]
        [Required]
        public string Nombres { get; set; }
        [DisplayName("Apellidos")]
        [Required]
        public string Apellidos { get; set; }
        [DisplayName("Correo Electrónico")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }       
        [DisplayName("Número de DPI")]
        [Required]
        [DataType(DataType.Text)]
        public string CUI { get; set; }
        [DisplayName("Departamento")]
        [Required]
        [DataType(DataType.Text)]
        public string Departamento { get; set; }
        public int IdRol { get; set; }
        public int IdCargo { get; set; }
        public int IdPuntoAtencion { get; set; }
        [DisplayName("Número de Cuenta")]
        [Required]
        [DataType(DataType.Text)]
        public string NumeroCuenta { get; set; }
        public string Estado { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != ConfirmPassword)
            {
                yield return new ValidationResult("Las contraseñas no coinciden", new[] { "ConfirmPassword", "Password" });
            }
        }
    }
}