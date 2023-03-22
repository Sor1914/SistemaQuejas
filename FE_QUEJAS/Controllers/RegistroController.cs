using FE_QUEJAS.Models;
using FE_QUEJAS.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace FE_QUEJAS.Controllers
{
    public class RegistroController : Controller
    {
        clsRegistroService _Registro = new clsRegistroService();
        string LayoutLogin = "~/Views/Shared/_LayoutLogin.cshtml";

        public RegistroController()
        {
            ViewBag.Layout = LayoutLogin;
        }
        // GET: Registro
        public ActionResult Registro()
        {            
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Registrarse(string btnRegistro, string btnRegresar, RegistroRequest Registro)
        {
            try
            {
                ViewBag.Layout = LayoutLogin;
                if (!string.IsNullOrEmpty(btnRegistro))
                {
                    Registro.IdRol = 1;
                    Registro.IdCargo = 1;
                    Registro.IdPuntoAtencion = 1;
                    Registro.Estado = "A";                    
                    bool registrado = await _Registro.registrarseApi(Registro);
                    if (registrado)
                    {                                               
                        return RedirectToAction("LoginWhenRegisterComplete", "Login");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ocurrió un error, intente de nuevo");
                        return View("Registro");
                    }
                }
                else if (!string.IsNullOrEmpty(btnRegresar))
                {
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    return View("Registro");
                }
            } catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error, intente de nuevo");
                return View("Registro");
            }
            
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

        private void mostrarMensaje(string mensaje, int tipo)
        {
            ViewBag.Mensaje = mensaje;
            ViewBag.Tipo = tipo;
            ViewBag.MostrarModal = true;                        
        }
    }
}