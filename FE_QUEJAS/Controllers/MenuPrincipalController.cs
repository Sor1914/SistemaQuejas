using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FE_QUEJAS.Controllers
{
    public class MenuPrincipalController : Controller
    {
        string LayoutMenu = "~/Views/Shared/_LayoutMenu.cshtml";
        public MenuPrincipalController()
        {
            ViewBag.Layout = LayoutMenu;
        }
        // GET: MenuInicial
        public ActionResult Bienvenida()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }
    }
}