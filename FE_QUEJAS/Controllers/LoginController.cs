using FE_QUEJAS.Models;
using FE_QUEJAS.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace FE_QUEJAS.Controllers
{
    public class LoginController : Controller
    {
        
        clsLoginService _Login = new clsLoginService();
        string LayoutLogin = "~/Views/Shared/_LayoutLogin.cshtml";

        public LoginController()
        {
            ViewBag.Layout = LayoutLogin;
        }
        public ActionResult Login()
        {
            ViewBag.Layout = LayoutLogin;
            return View();
        }

        public ActionResult LoginWhenRegisterComplete()
        {
            mostrarMensaje("Te has registrado correctamente, inicia sesión", 1);
            return View("Login");
        }

        public ActionResult Logout()
        {
            HttpCookieCollection cookies = Request.Cookies;
            foreach (string key in cookies.AllKeys)
            {
                HttpCookie cookie = cookies[key];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            Session.Clear();
            Session.Abandon();
            return View("Login");
        }

        [HttpPost]
        public async Task<ActionResult> iniciarSesion(string btnLogin, string btnRegistro, LoginRequest login)
        {
            string token;
            if (!string.IsNullOrEmpty(btnLogin))
            {
                try
                {
                    LoginRequest LoginResponse = new LoginRequest();
                    LoginResponse = await _Login.iniciarSesionApi(login);
                    if (LoginResponse.Token != "NE")
                    {
                        Permisos permiso = LoginResponse.permisos;
                        var cookie = new HttpCookie("TokenJwt", LoginResponse.Token);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);                       
                        Session["Usuario"] = login.Usuario;                        
                        Session["PuntoAtencion"] = permiso.CatalogoPuntosAtencion;
                        Session["Asignacion"] = permiso.AsignacionRechazo;
                        Session["UsuarioPuntoAtencion"] = permiso.UsuarioPuntoAtencion;
                        Session["AutoConsulta"] = permiso.AutoConsulta;
                        Session["IngresoQuejasUsuario"] = permiso.IngresoQuejasUsuario;
                        Session["IngresoQuejasCliente"] = permiso.IngresoQuejasCliente;
                        Session["Quejas"] = permiso.CatalogoQuejas;
                        Session["Reporte"] = permiso.Reporte;
                        Session["SeguimientoCentralizador"] = permiso.SeguimientoCentralizador;
                        Session["SeguimientoPuntoAtencion"] = permiso.SeguimientoPuntoAtencion;
                        Session["Usuarios"] = permiso.Usuarios;
                        return RedirectToAction("Bienvenida", "MenuPrincipal");
                    } else
                    {
                        ModelState.AddModelError(string.Empty, "Usuario y/o contraseña inválidos");
                        return View("Login");
                    }                                                            
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Usuario y/o contraseña inválidos");
                    return View("Login");
                }
            }
            else if (!string.IsNullOrEmpty(btnRegistro))
            {
                return RedirectToAction("Registro", "Registro");
            } else
            {
                return View("Login");
            }

        }
        private void mostrarMensaje(string mensaje, int tipo)
        {
            ViewBag.Mensaje = mensaje;
            ViewBag.Tipo = tipo;
            ViewBag.MostrarModal = true;
        }
    }
}
