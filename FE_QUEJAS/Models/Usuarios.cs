using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FE_QUEJAS.Models
{
    public class Usuarios
    {
        public int Id_Usuario { get; set; }
        public string Usuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string CUI { get; set; }
        public string Departamento { get; set; }
        public string Nombre_Rol { get; set; }
        public int Id_Rol { get; set; }
    }

    public class Roles
    {
        public int Id_Rol { get; set;}
        public string Nombre_Rol { get; set; }
    }
}