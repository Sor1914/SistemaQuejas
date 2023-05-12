using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FE_QUEJAS.Models
{
    public class UsuarioPuntoAtencion
    {
        public int Id_Usuario { get; set; }
        [Required]
        [DisplayName("DPI")]
        public string Cui { get; set; }
        [Required]
        [DisplayName("Nombre")]
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Nombre_Cargo { get; set; }
        public string Nombre_Punto_Atencion { get; set; }
        public string Nombre_Region { get; set; }
        [Required]
        [DisplayName("Punto de Atención")]
        public int Id_Punto_Atencion { get; set; }
        [Required]
        [DisplayName("Region")]
        public int Id_Region { get; set; }
        [Required]
        [DisplayName("Cargo")]
        public int Id_Cargo { get; set; }
        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }
        public string Usuario { get; set; }
        public string Estado { get; set; }

    }
}