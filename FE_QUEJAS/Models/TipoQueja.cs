using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FE_QUEJAS.Models
{
    public class TipoQueja
    {
        public int Id_Tipo { get; set; }
        [Required]
        [DisplayName("Siglas")]
        public string Siglas_Tipo { get; set; }
        [Required]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        public int Correlativo { get; set; }
        public string Estado { get; set; }
    }
}