using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FE_QUEJAS.Models
{
    public class Seguimiento
    {
    }

    public class EncabezadoQueja
    {
        public int Id_Encabezado { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Correlativo { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string Detalle { get; set; }
        public string Direccion_Archivo { get; set; }
        public string Usuario { get; set; }
        public int Id_Estado_Externo { get; set; }
        public string Estado_Externo { get; set; }
        public int Id_Estado_Interno { get; set; }
        public string Estado_Interno { get; set; }
        [Required]
        public string Justificacion { get; set; }
        public string Respuesta { get; set; }
        public int Id_Detalle { get; set; }     
        [Required]
        public string Comentario { get; set; }        
        public string Id_Usuario { get; set; }
        public string Nombre_Region { get; set; }
        public string Nombre_Punto_Atencion { get; set; }
        [Required]
        [DisplayName("Punto Atención")]
        public int Id_Punto_Atencion { get; set; }
        [Required]
        [DisplayName("Región")]
        public int Id_Region { get; set; }
        public DateTime Fecha_Ingreso { get; set; }
    }

    public class DetalleQueja
    {
        public int Id_Detalle { get; set; }
        public int Id_Encabezado { get; set; }
        public string Comentario { get; set; }
        public string Direccion_Archivo { get; set; }
        public string Id_Usuario { get; set; }
        public DateTime Fecha_Detalle { get; set; }
        public HttpPostedFileBase ArchivoAdjunto { get; set; }
    }
}