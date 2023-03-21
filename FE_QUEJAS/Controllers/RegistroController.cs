using FE_QUEJAS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace FE_QUEJAS.Controllers
{
    public class RegistroController : Controller
    {
        string LayoutLogin = "~/Views/Shared/_LayoutLogin.cshtml";
        // GET: Registro
        public ActionResult Registro()
        {
            ViewBag.Layout = LayoutLogin;
            return View();
        }

        public ActionResult registrarse(RegistroRequest Registro)
        {                  
            ViewBag.Layout = LayoutLogin;
            return View("Registro");
        }


        //[CustomValidation(typeof(RegistroController), "IsValidPass")]
        public static ValidationResult IsValidPass(string Password)
        {
            if (string.IsNullOrEmpty(Password))
            {
                return new ValidationResult("Las contraseñas no son identicas");
            }
            return ValidationResult.Success;
        }
    }
}