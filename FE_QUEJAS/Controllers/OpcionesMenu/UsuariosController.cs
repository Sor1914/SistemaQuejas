using FE_QUEJAS.Models;
using FE_QUEJAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace FE_QUEJAS.Controllers.OpcionesMenu
{
    public class UsuariosController : Controller
    {
        string token = "";
        string LayoutMenu = "~/Views/Shared/_LayoutMenu.cshtml";
        clsUsuariosService _Usuarios = new clsUsuariosService();
        // GET: Usuarios
        public UsuariosController()
        {
            ViewBag.Layout = LayoutMenu;
        }

        public async Task<ActionResult> Usuarios(bool limpiar = true)
        {
            if (limpiar)
            {
                HttpContext.Cache.Remove("listaUsuarios");
            }
            ViewBag.PaginaActual = ControllerContext.RouteData.Values["action"].ToString();
            await Paginacion();
            await llenarRoles(0);
            return PartialView("Usuarios");
        }

        public async Task<ActionResult> mostrarModalEditar(int Id_Usuario, string Nombre, string DPI, int id_Rol)
        {
            ViewBag.MostrarModalNuevo = true;
            ViewBag.Id_Usuario = Id_Usuario;
            ViewBag.Nombre = Nombre;
            ViewBag.DPI = DPI;
            ViewBag.Id_Rol = id_Rol;
            ViewBag.TituloModal = "Editar Rol";
            await llenarRoles(id_Rol);
            await Paginacion();
            return await Usuarios(false);
        }

        public async Task<ActionResult> notificarEliminacion(int id)
        {
            try
            {
                token = Request.Cookies["TokenJwt"]?.Value;
                ViewBag.IdEliminar = id;
                ViewBag.Eliminar = "True";
                string mensaje = "¿Está seguro de eliminar este usuario?";
                mostrarMensaje(mensaje, 4);                
                return await Usuarios(false);
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al eliminar la información", 3);                
                return await Usuarios(false);

            }
        }

        public async Task<ActionResult> eliminarUsuario(int id)
        {
            try
            {
                Usuarios usuario = new Usuarios { Id_Usuario = id };
                token = Request.Cookies["TokenJwt"]?.Value;
                bool eliminado = await _Usuarios.eliminarUsuario(usuario, token);
                if (eliminado)
                {
                    mostrarMensaje("El usuario se eliminó correctamente", 1);
                    HttpContext.Cache.Remove("listaUsuarios");
                }
                else
                    mostrarMensaje("Hubo un error al eliminar el registro", 2);                
                return await Usuarios();
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al eliminar la información", 3);                
                return await Usuarios(false);
            }
        }

        public async Task<ActionResult> actualizarRol(Usuarios usuario, int idUsuario)
        {
            try
            {
                usuario.Id_Usuario = idUsuario;
                token = Request.Cookies["TokenJwt"]?.Value;                
                bool respuesta = await _Usuarios.actualizarUsuario(usuario, token);
                if (respuesta)
                {                   
                    string mensajeNotificacion = "El rol se modificó correctamente";
                    mostrarMensaje(mensajeNotificacion, 1);
                }
                else
                    mostrarMensaje("Hubo un error al guardar la información", 3);
                return await Usuarios(true);
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al guardar la información", 3);
                return await Usuarios();
            }
        }

        private void mostrarMensaje(string mensaje, int tipo)
        {
            ViewBag.Mensaje = mensaje;
            ViewBag.Tipo = tipo;
            ViewBag.MostrarModal = true;
        }


        public async Task<ActionResult> Paginacion(int pagina = 1, int tamanoPagina = 5, string busqueda = "")
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<Usuarios> ListaUsuarios = new List<Usuarios>();
            List<Usuarios> ListaUsuariosFiltro = new List<Usuarios>();
            if (HttpContext.Cache["listaUsuarios"] != null)
                ListaUsuarios = HttpContext.Cache["listaUsuarios"] as List<Usuarios>;
            else
            {
                ListaUsuarios = await _Usuarios.obtenerUsuarios(token);
                HttpContext.Cache.Insert("listaUsuarios", ListaUsuarios, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }
            if (busqueda != "")
                ListaUsuariosFiltro = ListaUsuarios.Where(p => (p.Nombres + " " + p.Apellidos).ToUpper().Trim().Contains(busqueda.ToUpper().Trim())).ToList();
            else
                ListaUsuariosFiltro = ListaUsuarios;
            int totalElementos = ListaUsuariosFiltro.Count;
            var elementosPagina = ListaUsuariosFiltro.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina);
            ViewBag.PaginaActualTabla = pagina;
            ViewBag.TamanoPagina = tamanoPagina;
            ViewBag.TotalElementos = totalElementos;
            ViewBag.TotalPaginas = (int)Math.Ceiling((decimal)totalElementos / tamanoPagina);
            ViewBag.ListaTipos = elementosPagina;
            return PartialView("_Usuarios");
        }

        private async Task llenarRoles(int idRol)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<Roles> listaRoles = new List<Roles>();
            if (HttpContext.Cache["listaRoles"] as List<Roles> != null)
                listaRoles = HttpContext.Cache["listaRoles"] as List<Roles>;
            else
            {
                listaRoles = await _Usuarios.obtenerRoles(token);
                HttpContext.Cache.Insert("listaRoles", listaRoles, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }
            //obtener la lista de regiones
            var rolesSelectList = new List<SelectListItem>(); // Crear una lista vacía de objetos SelectListItem                                
            foreach (var rol in listaRoles)
            {
                // Agregar cada objeto Region a la lista de objetos SelectListItem
                if (idRol == rol.Id_Rol)
                    rolesSelectList.Add(new SelectListItem { Value = rol.Id_Rol.ToString(), Text = rol.Nombre_Rol, Selected = true });
                else
                    rolesSelectList.Add(new SelectListItem { Value = rol.Id_Rol.ToString(), Text = rol.Nombre_Rol });
            }
            ViewBag.RolSelectList = rolesSelectList;
        }

    }
}