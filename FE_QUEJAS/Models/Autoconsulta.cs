using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Recaptcha.Web.Mvc;
namespace FE_QUEJAS.Models
{
    public class Autoconsulta
    {
        [DisplayName("Tipo de Queja")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Tipo_Queja { get; set; }
        [DisplayName("No. Queja")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int Numero_Queja { get; set; }
        [DisplayName("Año de Queja")]

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int Anio_Queja { get; set; }
        public string Correlativo
        {
            get
            {
                return $"{Tipo_Queja}-{Numero_Queja}-{Anio_Queja}";
            }
        }

        public string RecaptchaResponse { get; set; }
    }
}