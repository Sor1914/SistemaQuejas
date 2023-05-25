using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FE_QUEJAS.Models
{
    public class Queja
    {        
        public string Correlativo { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]        
        public string Nombres { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Apellidos { get; set; }
        [DisplayName("Correo Electrónico")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Ingrese un correo electrónico válido.")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El número de teléfono no es válido")]
        [MaxLength(8)]
        public string Telefono { get; set; }
        public string Usuario { get; set; }
        [DisplayName("Detalle")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Detalle { get; set; }
        public int Estado_Externo { get; set; }
        public int Estado_Interno { get; set; }
        [DisplayName("Tipo de Queja")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int Tipo_Queja { get; set; }
        public int Id_Origen { get; set; }
        public string Direccion_Archivo { get; set; }
        public int Id_Punto_Atencion { get; set; }        
        public HttpPostedFileBase ArchivoAdjunto { get; set; }

    }

    public class Emails
    {
        public string Email { get; set; }
    }
}