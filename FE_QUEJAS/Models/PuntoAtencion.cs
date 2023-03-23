using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FE_QUEJAS.Models
{
    public class PuntoAtencion
    {
        public int Id { get; set; }
        public string NombrePuntoAtencion { get; set; }
        public int IdRegion { get; set; }
        public string Estado { get; set; }
    }
    public class Regiones
    {
        public int Id_Region { get; set; }
        public string Nombre_Region { get; set; }       
    }
}