using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FE_QUEJAS.Controllers
{
    public class HomeController : Controller
    {                        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult presionaBotonHome(string btnContacto, string btnLogin, string btnSeguimiento)
        {
            if (!string.IsNullOrEmpty(btnContacto))
            {
                return RedirectToAction("Login", "Login");
            }
            else if (!string.IsNullOrEmpty(btnLogin))
            {
               
                return RedirectToAction("Login", "Login");

            }
            else if (!string.IsNullOrEmpty(btnSeguimiento))
            {                
                return RedirectToAction("Login", "Login");
            } else
            {
                return View("Index");
            }           
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}