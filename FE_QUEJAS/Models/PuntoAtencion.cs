using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FE_QUEJAS.Models
{
    public class PuntoAtencion
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Punto de Atención")]

        public string NombrePuntoAtencion { get; set; }
        [Required]
        [DisplayName("Region")]
        public int IdRegion { get; set; }
        public string NombreRegion { get; set; }
        public string Estado { get; set; }
        public string busqueda { get; set; }
        public int cantidadUsuarios { get; set; }
    }
    public class Regiones
    {
        public int Id_Region { get; set; }
        public string Nombre_Region { get; set; }       
    }
}