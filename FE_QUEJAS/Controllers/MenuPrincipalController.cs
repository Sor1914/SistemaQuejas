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
        string LayoutMenu = "~/Views/Shared/_LayoutMenu.cshtml";
        
        public MenuPrincipalController()
        {
            int rol = 2;
            ViewBag.Layout = LayoutMenu;
            ViewBag.Rol = rol;
        }

        // GET: MenuInicial
        public ActionResult Bienvenida()
        {

            ViewBag.PaginaActual = ControllerContext.RouteData.Values["action"].ToString();
            ViewBag.Usuario = Session["usuario"].ToString();
            return View();
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