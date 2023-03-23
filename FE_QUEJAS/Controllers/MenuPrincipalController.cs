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
        string token="";

        public MenuPrincipalController()
        {
            if (Request.Cookies["TokenJwt"])
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
            //Obtener Token jwt
            token = Request.Cookies["TokenJwt"]?.Value;
            //obtener la lista de regiones
            List<Regiones> listaRegiones = await _Punto.obtenerRegionesApi(token);
            var regionesSelectList = new List<SelectListItem>(); // Crear una lista vacía de objetos SelectListItem
            foreach (var region in listaRegiones)            
                // Agregar cada objeto Region a la lista de objetos SelectListItem
                regionesSelectList.Add(new SelectListItem { Value = region.Id_Region.ToString(), Text = region.Nombre_Region });
            ViewBag.RegionesSelectList = regionesSelectList; 
            return View();
        }

        private void llenarDdlRegiones()
        {
            

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