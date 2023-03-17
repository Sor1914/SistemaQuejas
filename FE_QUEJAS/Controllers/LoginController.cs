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
        public ActionResult Login()
        {
            ViewBag.Layout = LayoutLogin;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> iniciarSesion(string btnLogin, string btnRegistro, LoginRequest login)
        {
            string token;
            if (!string.IsNullOrEmpty(btnLogin))
            {
                try
                {
                    token = await _Login.iniciarSesionApi(login);
                    if (token != "NE")
                    {                        
                        var cookie = new HttpCookie("TokenJwt", token);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);                       
                        Session["Usuario"] = login.Usuario;                        
                        return View("Menuprincipal");
                    } else
                    {
                        ModelState.AddModelError(string.Empty, "Usuario y/o contraseña inválidos");
                        return View("Login");
                    }                                                            
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Usuario y/o contraseña inválidos");
                    return View();
                }
            }
            else if (!string.IsNullOrEmpty(btnRegistro))
            {
                return View("Login");
            } else
            {
                return View("Login");
            }

        }

        
    }
}
