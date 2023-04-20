using FE_QUEJAS.Models;
using FE_QUEJAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FE_QUEJAS.Controllers
{
    public class MenuPrincipalController : Controller
    {
        clsPuntoAtencionService _Punto = new clsPuntoAtencionService();


        string LayoutMenu = "~/Views/Shared/_LayoutMenu.cshtml";
        string token = "";

        public MenuPrincipalController()
        {
            ViewBag.Layout = LayoutMenu;
        }

        // GET: MenuInicial
        public ActionResult Bienvenida()
        {

            ViewBag.PaginaActual = ControllerContext.RouteData.Values["action"].ToString();
            ViewBag.Usuario = Session["usuario"].ToString();
            return View();
        }

        public async Task<ActionResult> PuntoAtencion()
        {
            //Validar sesion de usuario
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            //Verificar página abierta actualmente
            ViewBag.PaginaActual = ControllerContext.RouteData.Values["action"].ToString();            
            return await Paginacion();
        }

        public async Task<ActionResult> Paginacion(int idRegion = 0, int pagina = 1, int tamanoPagina = 5, string busqueda = "")
        {            
            token = Request.Cookies["TokenJwt"]?.Value;
            await llenarRegiones(idRegion);
            PuntoAtencion Punto = new PuntoAtencion { Id_Region = idRegion, busqueda=busqueda};
            List<PuntoAtencion> listaPuntos = await _Punto.obtenerPuntosApi(Punto,token);
            int totalElementos = listaPuntos.Count;
            var elementosPagina = listaPuntos.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina);
            ViewBag.ListaPuntos = elementosPagina;
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
            //obtener la lista de regiones
            List<Regiones> listaRegiones = await _Punto.obtenerRegionesApi(token);
            var regionesSelectList = new List<SelectListItem>(); // Crear una lista vacía de objetos SelectListItem
            foreach (var region in listaRegiones)
            {
                // Agregar cada objeto Region a la lista de objetos SelectListItem
                if (idRegion == region.Id_Region)
                    regionesSelectList.Add(new SelectListItem { Value = region.Id_Region.ToString(), Text = region.Nombre_Region, Selected = true });
                else
                    regionesSelectList.Add(new SelectListItem { Value = region.Id_Region.ToString(), Text = region.Nombre_Region});
            }                
            ViewBag.RegionesSelectList = regionesSelectList;
        }

        public async Task<ActionResult> CambiarRegion(int regionId)
        {            
            return await Paginacion(regionId);
        }

        private void validarPermisos()
        {
            //Si es admin
            if (true)
            {
                ViewBag.Permiso = true;
            }
        }
    }

}