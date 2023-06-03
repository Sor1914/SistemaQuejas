using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FE_QUEJAS.Models
{
    public class Reporte
    {
        [DisplayName("Fecha Inicial")]
        public DateTime? Fecha_Inicial { get; set; }
        [DisplayName("Fecha Final")]
        public DateTime? Fecha_Final { get; set; }
        [DisplayName("No. Queja")]
        public string Numero_Queja { get; set; }
        [DisplayName("Region")]
        public int? id_Region { get; set; }
        [DisplayName("Punto de Atención")]
        public int? id_Punto_Atencion { get; set; }

    }

    public class EncabezadoQuejaReporte
    {
        public string Correlativo { get; set; }
        public string Nombre_Tipo_Queja { get; set; }
        public string Nombre_Punto_Atencion { get; set; }
        public string Estado_Interno { get; set; }
        public string Estado_Externo { get; set; }
        public string Justificacion { get; set; }
        public string Detalle { get; set; }
        public string Nombre_Origen { get; set; }
        public DateTime Fecha_Ingreso { get; set; }
    }


}