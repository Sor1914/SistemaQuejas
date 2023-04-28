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
        clsEnvioEmail _Correo = new clsEnvioEmail();
        clsRegistroService _Registro = new clsRegistroService();
        string LayoutLogin = "~/Views/Shared/_LayoutLogin.cshtml";

        public RegistroController()
        {
            ViewBag.Layout = LayoutLogin;
        }
        // GET: Registro
        public ActionResult Registro()
        {
            llenarDepartamentos();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Registrarse(string btnRegistro, string btnRegresar, RegistroRequest Registro)
        {
            try
            {
                llenarDepartamentos();
                ViewBag.Layout = LayoutLogin;
                if (!string.IsNullOrEmpty(btnRegistro))
                {
                    Registro.IdRol = 4;
                    Registro.IdCargo = 1;
                    Registro.IdPuntoAtencion = 1;
                    Registro.Estado = "A";
                    string registrado = await _Registro.registrarseApi(Registro);
                    if (registrado == "Ok")
                    {
                        string[] correo = new string[1];
                        correo[0] = Registro.Email;
                        //Enviar correo de bienvenida
                        _Correo.enviarCorreo(correo, "Bienvenido", crearHtmlRegistro(Registro.Nombres, Registro.Usuario));
                        return RedirectToAction("LoginWhenRegisterComplete", "Login");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, registrado);
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
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error, intente de nuevo");
                return View("Registro");
            }

        }

        private void llenarDepartamentos()
        {
            var departamentosSelectList = new List<SelectListItem>(); // Crear una lista vacía de objetos SelectListItem    
            var departamentos = new List<string>
                {
                    "Alta Verapaz",
                    "Baja Verapaz",
                    "Chimaltenango",
                    "Chiquimula",
                    "El Progreso",
                    "Escuintla",
                    "Guatemala",
                    "Huehuetenango",
                    "Izabal",
                    "Jalapa",
                    "Jutiapa",
                    "Petén",
                    "Quetzaltenango",
                    "Quiché",
                    "Retalhuleu",
                    "Sacatepéquez",
                    "San Marcos",
                    "Santa Rosa",
                    "Sololá",
                    "Suchitepéquez",
                    "Totonicapán",
                    "Zacapa"
                };
            // Agregar cada objeto Region a la lista de objetos SelectListItem            
            //departamentosSelectList.Add(new SelectListItem { Value = "Guatemala", Text = "Guatemala", Selected = true });            
            foreach (var departamento in departamentos)
            {
                departamentosSelectList.Add(new SelectListItem { Value = departamento, Text = departamento });
            }
            ViewBag.Departamentos = departamentosSelectList;
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

        private string crearHtmlRegistro(string nombres, string usuario)
        {
            string html = "<center> <div style='background: #eeeeee; width:520px; border-radius: 0; padding: 20px;'> <table align='center'> <tbody> <tr> <td colspan='2'> <h3 style='text-align: center;'>Registro Correcto</h3> <p style='text-align: left;'>Hola {0}, Te damos la bienvenida al sistema de quejas del banco Mi Pretamito</p> <p style='text-align: left;'>Esperamos poder apoyarte en todo lo que necesites y te sientas cómodo de expresar tus incomodidades.</p>" +
                "</td> <</tr> <tr> <td colspan='0'> Usuario </td> <td colspan='1'> {1} </td> </tr> <tr> <td colspan='2'> <center> <a href='https://google.com' style='display:inline-block; text-decoration: none; color: black;'> <div style='width: 150px; height: 50px; background-color: rgb(0, 0, 0); border-radius: 10px;'> <h3 style=' text-align: center;  line-height: 50px; color:white;'> Visitar </h3> </div> </a> </center> </br> </td> </tr> </tbody> </table> </center>";
            string htmlFinal = string.Format(html, nombres, usuario);
            return htmlFinal;            
        }
    }
}