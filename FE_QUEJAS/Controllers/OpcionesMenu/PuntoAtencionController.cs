using FE_QUEJAS.Models;
using FE_QUEJAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace FE_QUEJAS.Controllers.OpcionesMenu
{
    public class PuntoAtencionController : Controller
    {
        string token = "";
        string LayoutMenu = "~/Views/Shared/_LayoutMenu.cshtml";
        clsPuntoAtencionService _Punto = new clsPuntoAtencionService();
        public PuntoAtencionController()
        {
            ViewBag.Layout = LayoutMenu;
        }
        // GET: PuntoAtencion
        public async Task<ActionResult> PuntoAtencion()
        {
            //Validar sesion de usuario
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            //Verificar página abierta actualmente
            ViewBag.PaginaActual = ControllerContext.RouteData.Values["action"].ToString();
            HttpContext.Cache.Remove("listaPuntos");
            HttpContext.Cache.Remove("listaRegiones");
            return await Paginacion();
        }

        public async Task<ActionResult> mostrarModalNuevo()
        {
            ViewBag.MostrarModalNuevo = true;
            ViewBag.ModalAccion = "nuevo";
            ViewBag.TituloModal = "Creación de Puntos de Atención";
            ViewBag.Id = 0;
            return await Paginacion();
        }

        public async Task<ActionResult> mostrarModalEditar(int id, string nombrePuntoAtencion, int idRegion)
        {
     
            ViewBag.MostrarModalNuevo = true;
            ViewBag.ModalAccion = "editar";
            ViewBag.TituloModal = "Editar Punto de Atención";
            ViewBag.NombrePuntoAtencion = nombrePuntoAtencion;
            ViewBag.Id = id;
            return await Paginacion();
        }

        public async Task<ActionResult> guardarPuntoAtencion(string tipo, int id, PuntoAtencion Punto)
        {
            try
            {
                Punto.Id = id;
                token = Request.Cookies["TokenJwt"]?.Value;
                bool guardado = false;
                string mensajeNotificacion = "";
                if (tipo == "nuevo")
                {
                    guardado = await _Punto.agregarPuntoApi(Punto, token, "administrador");
                    mensajeNotificacion = "La información se almacenó correctamente";
                } else
                {
                    guardado = await _Punto.editarPuntoApi(Punto, token);
                    mensajeNotificacion = "La información se actualizó correctamente";
                }
                if (guardado)
                {
                    mostrarMensaje(mensajeNotificacion, 1);
                    HttpContext.Cache.Remove("listaPuntos");
                }
                else
                {
                    mostrarMensaje("Hubo un error al guardar la información", 3);
                }
                return await Paginacion();
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al guardar la información", 3);
                return await Paginacion();
            }
        }


        public async Task<ActionResult> notificarEliminacion(int id)
        {
            try
            {
                PuntoAtencion Punto = new PuntoAtencion {Id = id};
                token = Request.Cookies["TokenJwt"]?.Value;
                int cantidadUsuarios = await _Punto.contarPuntosAtencion(Punto,token);
                ViewBag.IdEliminar = Punto.Id;
                ViewBag.CantidadUsuarios = cantidadUsuarios;
                ViewBag.Eliminar = "True";
                string mensaje = "¿Está seguro de inactivar el Punto de Atención?";
                if (cantidadUsuarios>0)                 
                    mensaje = "Existen " + cantidadUsuarios + " usuarios asociados al punto de atención, TODOS los usuarios serán automáticamente Inactivados. ¿Continua con el proceso de Inactivación del Punto de Atención?";                                  
                mostrarMensaje(mensaje, 4);
                return await Paginacion();
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al eliminar la información", 3);
                return await Paginacion();

            }
        }

        public async Task<ActionResult> eliminarPuntoAtencion(int id, int cantidadUsuarios)
        {
            try
            {
                PuntoAtencion Punto = new PuntoAtencion{Id = id};
                token = Request.Cookies["TokenJwt"]?.Value;
                if (cantidadUsuarios > 0) await _Punto.inactivarUsuariosPuntoAtencion(Punto, token);                                
                bool eliminado = await _Punto.eliminarPuntoApi(Punto, token);
                if (eliminado)
                {
                    mostrarMensaje("El registro se eliminó correctamente", 1);
                    HttpContext.Cache.Remove("listaPuntos");
                }
                else
                    mostrarMensaje("Hubo un error al eliminar el registro", 2);
                
                return await Paginacion();
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al eliminar la información", 3);
                return await Paginacion();

            }
        }

        public async Task<ActionResult> Paginacion(int idRegion = 0, int pagina = 1, int tamanoPagina = 5, string busqueda = "")
        {
            ViewBag.PaginaActual = ControllerContext.RouteData.Values["action"].ToString();

            token = Request.Cookies["TokenJwt"]?.Value;
            await llenarRegiones(idRegion);
            PuntoAtencion Punto = new PuntoAtencion { };
            List<PuntoAtencion> listaPuntos = new List<PuntoAtencion>();
            List<PuntoAtencion> listaPuntosFiltro = new List<PuntoAtencion>();
            if (HttpContext.Cache["listaPuntos"] != null)
                listaPuntos = HttpContext.Cache["listaPuntos"] as List<PuntoAtencion>;
            else
            {
                listaPuntos = await _Punto.obtenerPuntosApi(Punto, token, "administrador");
                HttpContext.Cache.Insert("listaPuntos", listaPuntos, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }
            if (idRegion != 0)
                listaPuntosFiltro = listaPuntos.Where(p => p.IdRegion == idRegion).ToList();
            else
                listaPuntosFiltro = listaPuntos;
            if (busqueda != "")
                listaPuntosFiltro = listaPuntosFiltro.Where(p => p.NombrePuntoAtencion.ToUpper().Trim().Contains(busqueda.ToUpper().Trim())).ToList();
            int totalElementos = listaPuntosFiltro.Count;
            var elementosPagina = listaPuntosFiltro.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina);
            ViewBag.ListaPuntos = elementosPagina;
            if (elementosPagina.Count() > 0)
                ViewBag.ListaPuntos = elementosPagina;
            else
                ViewBag.ListaPuntos = null;
            ViewBag.PaginaActualTabla = pagina;
            ViewBag.TamanoPagina = tamanoPagina;
            ViewBag.TotalElementos = totalElementos;
            ViewBag.TotalPaginas = (int)Math.Ceiling((decimal)totalElementos / tamanoPagina);
            ViewBag.IdRegion = idRegion;
            ViewBag.Busqueda = busqueda;
            return View("PuntoAtencion");
        }

        private async Task llenarRegiones(int idRegion)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<Regiones> listaRegiones = new List<Regiones>();
            if (HttpContext.Cache["listaRegiones"] != null)
                listaRegiones = HttpContext.Cache["listaRegiones"] as List<Regiones>;
            else
            {
                listaRegiones =  await _Punto.obtenerRegionesApi(token);
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

        private void mostrarMensaje(string mensaje, int tipo)
        {
            ViewBag.Mensaje = mensaje;
            ViewBag.Tipo = tipo;
            ViewBag.MostrarModal = true;
        }
    }
}