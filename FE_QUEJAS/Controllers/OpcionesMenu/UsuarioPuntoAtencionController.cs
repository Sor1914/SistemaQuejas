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
    public class UsuarioPuntoAtencionController : Controller
    {
        string token = "";
        string LayoutMenu = "~/Views/Shared/_LayoutMenu.cshtml";
        clsUsuarioPuntoAtencionService _Usuario = new clsUsuarioPuntoAtencionService();

        public UsuarioPuntoAtencionController()
        {
            ViewBag.Layout = LayoutMenu;
        }

        public async Task<ActionResult> UsuarioPuntoAtencion()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            //Verificar página abierta actualmente        
            ViewBag.PaginaActual = ControllerContext.RouteData.Values["action"].ToString();
            HttpContext.Cache.Remove("listaPuntos");
            HttpContext.Cache.Remove("listaRegiones");
            HttpContext.Cache.Remove("listaUsuarios");
            ViewBag.PuestosSelectList = new List<SelectListItem>();
            await Paginacion();
            return PartialView("UsuarioPuntoAtencion");
        }

 

        public async Task<ActionResult> Paginacion(int idRegion = 0, int idPuntoAtencion = 0, int pagina = 1, int tamanoPagina = 5, string busqueda = "")
        {
            ViewBag.PaginaActual = ControllerContext.RouteData.Values["action"].ToString();
            token = Request.Cookies["TokenJwt"]?.Value;
            await llenarRegiones(idRegion);
            await llenarPuntos();
            await llenarPuestos();
            UsuarioPuntoAtencion Usuario = new UsuarioPuntoAtencion { };
            List<UsuarioPuntoAtencion> ListaUsuarios = new List<UsuarioPuntoAtencion>();
            List<UsuarioPuntoAtencion> ListaUsuariosFiltro = new List<UsuarioPuntoAtencion>();
            if (HttpContext.Cache["listaUsuarios"] != null)
                ListaUsuarios = HttpContext.Cache["listaUsuarios"] as List<UsuarioPuntoAtencion>;
            else
            {
                ListaUsuarios = await _Usuario.obtenerusuariosApi(token);
                HttpContext.Cache.Insert("listaUsuarios", ListaUsuarios, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }
            if (idRegion != 0)
                ListaUsuariosFiltro = ListaUsuarios.Where(p => p.Id_Region == idRegion).ToList();
            else
                ListaUsuariosFiltro = new List<UsuarioPuntoAtencion>();

            if (idPuntoAtencion != 0)
                ListaUsuariosFiltro = ListaUsuariosFiltro.Where(p => p.Id_Punto_Atencion == idPuntoAtencion).ToList();

            if (busqueda!="")
                ListaUsuariosFiltro = ListaUsuariosFiltro.Where(p => p.Cui.ToUpper().Trim().Contains(busqueda.ToUpper().Trim())).ToList();
            
            int totalElementos = ListaUsuariosFiltro.Count;
            var elementosPagina = ListaUsuariosFiltro.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina);            
            ViewBag.PaginaActualTabla = pagina;
            ViewBag.TamanoPagina = tamanoPagina;
            ViewBag.TotalElementos = totalElementos;
            ViewBag.TotalPaginas = (int)Math.Ceiling((decimal)totalElementos / tamanoPagina);            
            ViewBag.ListaUsuarioPunto = elementosPagina;
            return PartialView("_TablaUsuarios");
        }

        private async Task llenarRegiones(int idRegion)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<Regiones> listaRegiones = new List<Regiones>();
            if (HttpContext.Cache["listaRegiones"] as List<Regiones> != null)
                listaRegiones = HttpContext.Cache["listaRegiones"] as List<Regiones>;
            else
            {
                listaRegiones = await _Usuario.obtenerRegionesApi(token);
                HttpContext.Cache.Insert("listaRegiones", listaRegiones, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }
            //obtener la lista de regiones
            var regionesSelectList = new List<SelectListItem>(); // Crear una lista vacía de objetos SelectListItem                                
            foreach (var region in listaRegiones)
            {
                // Agregar cada objeto Region a la lista de objetos SelectListItem
                if (idRegion == region.Id_Region)
                    regionesSelectList.Add(new SelectListItem { Value = region.Id_Region.ToString(), Text = region.Nombre_Region, Selected = true });
                else
                    regionesSelectList.Add(new SelectListItem { Value = region.Id_Region.ToString(), Text = region.Nombre_Region });
            }
            ViewBag.RegionesSelectList = regionesSelectList;
        }

        private async Task llenarPuestos(int idPuesto = 0)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<Cargos> listaPuestos = new List<Cargos>();
            if (HttpContext.Cache["listaPuestos"] != null)
                listaPuestos = HttpContext.Cache["listaPuestos"] as List<Cargos>;
            else
            {
                listaPuestos = await _Usuario.obtenerCargosApi(token);
                HttpContext.Cache.Insert("listaRegiones", listaPuestos, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }
            //obtener la lista de regiones
            var puestosSelectList = new List<SelectListItem>(); // Crear una lista vacía de objetos SelectListItem                                
            foreach (var puesto in listaPuestos)
            {
                // Agregar cada objeto Region a la lista de objetos SelectListItem
                if (idPuesto == puesto.id_Area)
                    puestosSelectList.Add(new SelectListItem { Value = puesto.id_Area.ToString(), Text = puesto.Nombre_Cargo, Selected = true });
                else
                    puestosSelectList.Add(new SelectListItem { Value = puesto.id_Area.ToString(), Text = puesto.Nombre_Cargo});
            }
            ViewBag.PuestosSelectList = puestosSelectList;
        }

        private async Task llenarPuntos()
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<PuntoAtencion> listaPuntos = new List<PuntoAtencion>();
            List<PuntoAtencion> listaPuntosFiltro = new List<PuntoAtencion>();
            var puntosSelectList = new List<SelectListItem>(); // Crear una lista vacía de objetos SelectListItem
            if (HttpContext.Cache["listaPuntos"] != null)
                listaPuntos = HttpContext.Cache["listaPuntos"] as List<PuntoAtencion>;
            else
            {
                listaPuntos = await _Usuario.obtenerPuntosApi(token);
                HttpContext.Cache.Insert("listaPuntos", listaPuntos, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }                                   
        }

        public async Task<ActionResult> mostrarModalNuevo()
        {
            token = Request.Cookies["TokenJwt"]?.Value;            
            ViewBag.MostrarModalNuevo = true;
            ViewBag.ModalAccion = "nuevo";
            ViewBag.TituloModal = "Creación de Puntos de Atención";
            ViewBag.Id = 0;
            await Paginacion();
            await llenarPuestos();
            return PartialView("UsuarioPuntoAtencion");
        }

        public JsonResult ObtenerPuntosDeRegion(int idRegion = 0)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<PuntoAtencion> listaPuntos = new List<PuntoAtencion>();
            List<PuntoAtencion> listaPuntosFiltro = new List<PuntoAtencion>();
            var puntosSelectList = new List<SelectListItem>(); // Crear una lista vacía de objetos SelectListItem
            if (idRegion != 0)
            {
                listaPuntos = HttpContext.Cache["listaPuntos"] as List<PuntoAtencion>;
                //Filtrar por región                
                listaPuntosFiltro = listaPuntos.Where(p => p.IdRegion == idRegion).ToList();
                //obtener la lista de regiones                
                foreach (var punto in listaPuntosFiltro)
                {
                    // Agregar cada objeto Region a la lista de objetos SelectListItem                    
                    puntosSelectList.Add(new SelectListItem { Value = punto.Id.ToString(), Text = punto.NombrePuntoAtencion });
                }
            }
            var puntos = puntosSelectList;
            return Json(puntos, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> ObtenerDatosDeUsuario(string cui)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<UsuarioPuntoAtencion> Usuarios = new List<UsuarioPuntoAtencion>();
            UsuarioPuntoAtencion usuario = new UsuarioPuntoAtencion { Cui = cui };
            var puntosSelectList = new List<SelectListItem>(); // Crear una lista vacía de objetos SelectListItem
            if (cui != "")
            {
                Usuarios = await _Usuario.obtenerUsuarioPorCui(token, usuario);                             
            }            
            return Json(Usuarios, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> guardarUsuarioPuntoAtencion(string tipo, UsuarioPuntoAtencion Usuario)
        {
            try
            {                
                token = Request.Cookies["TokenJwt"]?.Value;
                bool guardado = false;
                string mensajeNotificacion = "";
                if (tipo == "nuevo")
                {
                    guardado = await _Usuario.actualizarDatosUsuario(Usuario, token);
                    mensajeNotificacion = "La información se almacenó correctamente";
                }
                else
                {
                    guardado = await _Usuario.actualizarDatosUsuario(Usuario, token);
                    mensajeNotificacion = "La información se actualizó correctamente";
                }
                if (guardado)
                {
                    mostrarMensaje(mensajeNotificacion, 1);                    
                }
                else
                {
                    mostrarMensaje("Hubo un error al guardar la información", 3);
                }
                return await UsuarioPuntoAtencion();
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al guardar la información", 3);
                return PartialView("UsuarioPuntoAtencion");
            }
        }

        private void mostrarMensaje(string mensaje, int tipo)
        {
            ViewBag.Mensaje = mensaje;
            ViewBag.Tipo = tipo;
            ViewBag.MostrarModal = true;
        }

        public async Task<ActionResult> mostrarModalEditar(string cui, string nombres, string email, int id_Region, int id_Punto, int id_Cargo)
        {
            ViewBag.MostrarModalNuevo = true;
            ViewBag.IdPuntoModal = id_Punto;
            ViewBag.ModalAccion = "editar"; 
            ViewBag.TituloModal = "Editar Punto de Atención";
            await llenarPuestos(id_Cargo);
            await Paginacion();
            return PartialView("UsuarioPuntoAtencion");
        }

        public async Task<ActionResult> notificarEliminacion(string id)
        {
            try
            {                
                token = Request.Cookies["TokenJwt"]?.Value;
                ViewBag.IdEliminar = id;
                ViewBag.Eliminar = "True";
                string mensaje = "¿Está seguro de eliminar el usuario del punto de atención?";               
                mostrarMensaje(mensaje, 4);
                await Paginacion();
                return PartialView("UsuarioPuntoAtencion");
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al eliminar la información", 3);
                await Paginacion();
                return PartialView("UsuarioPuntoAtencion");

            }
        }
        public async Task<ActionResult> eliminarUsuario(string id)
        {
            try
            {
                UsuarioPuntoAtencion usuario = new UsuarioPuntoAtencion { Cui = id, Id_Punto_Atencion=1, Id_Cargo =1 };                
                token = Request.Cookies["TokenJwt"]?.Value;               
                bool eliminado = await _Usuario.actualizarDatosUsuario(usuario, token);
                if (eliminado)
                {
                    mostrarMensaje("El registro se eliminó correctamente", 1);
                    HttpContext.Cache.Remove("listaPuntos");
                }
                else
                    mostrarMensaje("Hubo un error al eliminar el registro", 2);
                await Paginacion();
                return await UsuarioPuntoAtencion();
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al eliminar la información", 3);
                await Paginacion();
                return PartialView("UsuarioPuntoAtencion");
            }
        }

    }
}