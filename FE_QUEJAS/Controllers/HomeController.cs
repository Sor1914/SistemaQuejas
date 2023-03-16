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
            return RedirectToAction("Login", "Login");

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