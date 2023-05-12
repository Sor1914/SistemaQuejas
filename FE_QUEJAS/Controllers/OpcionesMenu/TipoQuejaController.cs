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
    public class TipoQuejaController : Controller
    {
        string token = "";
        string LayoutMenu = "~/Views/Shared/_LayoutMenu.cshtml";
        clsTipoQuejaService _TipoQueja = new clsTipoQuejaService();
        public TipoQuejaController()
        {
            ViewBag.Layout = LayoutMenu;

        }

        public async Task<ActionResult> TipoQueja(bool limpiar = true)
        {   
            if (limpiar)
            {
                HttpContext.Cache.Remove("listaTipos");

            }
            ViewBag.PaginaActual = ControllerContext.RouteData.Values["action"].ToString();
            await Paginacion();
            return PartialView("TipoQueja");
        }

        public async Task<ActionResult> mostrarModalNuevo()
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            ViewBag.MostrarModalNuevo = true;
            ViewBag.ModalAccion = "nuevo";
            ViewBag.TituloModal = "Creación de Puntos de Atención";
            ViewBag.Id = 0;
            return await TipoQueja(false);
        }

        public async Task<ActionResult> guardarTipoQueja(string tipo, TipoQueja tipoQueja, int id = 0)
        {
            try
            {
                token = Request.Cookies["TokenJwt"]?.Value;
                tipoQueja.Id_Tipo = id;
                bool guardado = false;
                string mensajeNotificacion = "";
                if (tipo == "nuevo")
                {
                    if (!await validarExistencia(tipoQueja.Siglas_Tipo))
                    {
                        mostrarMensaje("Las siglas para la queja que ingresó, ya fueron registradas previamente en el sistema, verifique.", 2);
                        return await TipoQueja(false);
                    } else
                    {
                        guardado = await _TipoQueja.insertarTipoQuejaApi(tipoQueja, token);
                        mensajeNotificacion = "El tipo de queja " +
                           tipoQueja.Siglas_Tipo + " - " + tipoQueja.Nombre + ", fue guardado correctamente";
                    }                    
                }
                else
                {
                    guardado = await _TipoQueja.actualizarTipoQuejaApi(tipoQueja, token);
                    mensajeNotificacion = "Datos actualizados";
                }
                if (guardado)
                {
                    mostrarMensaje(mensajeNotificacion, 1);
                }
                else
                {
                    mostrarMensaje("Hubo un error al guardar la información", 3);
                }
                return await TipoQueja();
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al guardar la información", 3);
                return await TipoQueja(false);
            }
        }

        private async  Task<bool> validarExistencia(string siglas)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<TipoQueja> ListaTipos = new List<TipoQueja>();
            List<TipoQueja> ListaTiposFiltro = new List<TipoQueja>();
            if (HttpContext.Cache["listaTipos"] != null)
                ListaTipos = HttpContext.Cache["listaTipos"] as List<TipoQueja>;
            else
            {
                ListaTipos = await _TipoQueja.obtenerTiposQueja(token);
                HttpContext.Cache.Insert("listaTipos", ListaTipos, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }            
            ListaTiposFiltro = ListaTipos.Where(p => p.Siglas_Tipo == siglas.ToUpper().Trim()).ToList();
            if (ListaTiposFiltro.Count() > 0)
                return false;
            else
                return true;
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
            List<TipoQueja> ListaTipos = new List<TipoQueja>();
            List<TipoQueja> ListaTiposFiltro = new List<TipoQueja>();
            if (HttpContext.Cache["listaTipos"] != null)
                ListaTipos = HttpContext.Cache["listaTipos"] as List<TipoQueja>;
            else
            {
                ListaTipos = await _TipoQueja.obtenerTiposQueja(token);
                HttpContext.Cache.Insert("listaTipos", ListaTipos, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }
            if (busqueda != "")
                ListaTiposFiltro = ListaTipos.Where(p => p.Siglas_Tipo.ToUpper().Trim().Contains(busqueda.ToUpper().Trim())).ToList();
            else
                ListaTiposFiltro = ListaTipos;
            int totalElementos = ListaTiposFiltro.Count;
            var elementosPagina = ListaTiposFiltro.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina);
            ViewBag.PaginaActualTabla = pagina;
            ViewBag.TamanoPagina = tamanoPagina;
            ViewBag.TotalElementos = totalElementos;
            ViewBag.TotalPaginas = (int)Math.Ceiling((decimal)totalElementos / tamanoPagina);
            ViewBag.ListaTipos = elementosPagina;
            return PartialView("_TablaQuejas");
        }

        public async Task<ActionResult> mostrarModalEditar(int Id_Tipo, string Nombre, string Siglas_Tipo)
        {
            ViewBag.MostrarModalNuevo = true;
            ViewBag.IdTipo = Id_Tipo;
            ViewBag.ModalAccion = "editar";
            ViewBag.TituloModal = "Editar Tipo Queja";            
            await Paginacion();
            return await TipoQueja(false);
        }

        public async Task<ActionResult> notificarEliminacion(int id)
        {
            try
            {
                token = Request.Cookies["TokenJwt"]?.Value;
                ViewBag.IdEliminar = id;
                ViewBag.Eliminar = "True";
                string mensaje = "¿Está seguro de eliminar el tipo de queja?";
                mostrarMensaje(mensaje, 4);
                await Paginacion();
                return await TipoQueja(false);
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al eliminar la información", 3);
                await Paginacion();
                return await TipoQueja(false);

            }
        }

        public async Task<ActionResult> eliminarTipo(int id)
        {
            try
            {
                TipoQueja tipoQueja = new TipoQueja { Id_Tipo = id };
                token = Request.Cookies["TokenJwt"]?.Value;
                bool eliminado = await _TipoQueja.eliminarTipoQueja(tipoQueja, token);
                if (eliminado)
                {
                    mostrarMensaje("El registro se eliminó correctamente", 1);
                    HttpContext.Cache.Remove("listaPuntos");
                }
                else
                    mostrarMensaje("Hubo un error al eliminar el registro", 2);
                await Paginacion();
                return await TipoQueja();
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al eliminar la información", 3);
                await Paginacion();
                return await TipoQueja(false);
            }
        }
    }
}
