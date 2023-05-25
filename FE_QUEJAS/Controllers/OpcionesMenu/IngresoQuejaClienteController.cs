using FE_QUEJAS.Models;
using FE_QUEJAS.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace FE_QUEJAS.Controllers.OpcionesMenu
{
    public class IngresoQuejaClienteController : Controller
    {
        string token = "";
        string LayoutMenu = "~/Views/Shared/_LayoutMenu.cshtml";
        clsTipoQuejaService _TipoQueja = new clsTipoQuejaService();
        clsQuejaService _Queja = new clsQuejaService();
        clsEnvioEmail _Email = new clsEnvioEmail();
        public IngresoQuejaClienteController()
        {
            ViewBag.Layout = LayoutMenu;
        }

        public async Task<ActionResult> IngresoQuejaCliente()
        {      
            ViewBag.PaginaActual = ControllerContext.RouteData.Values["action"].ToString();
            await llenarTipos();
           // await Paginacion();
            return View("IngresoQuejaCliente");
        }

        public async Task<ActionResult> guardarQueja(Queja queja)
        {
            try
            {                
                token = Request.Cookies["TokenJwt"]?.Value;
                HttpPostedFileBase archivoAdjunto = Request.Files["ArchivoAdjunto"];
                if (archivoAdjunto != null && archivoAdjunto.ContentLength > 0)
                {
                    queja.ArchivoAdjunto = archivoAdjunto;
                }
                queja = await _Queja.agregarQuejaApi(queja, token);
                if (queja != null)
                {
                    //Enviar correo interno
                    string[] correoExterno = new string[1];
                    correoExterno[0] = queja.Email;
                    string htmlExterno = crearHtmlConfirmacionExterno(queja.Correlativo);
                    _Email.enviarCorreo(correoExterno, "Queja Ingresada - " + queja.Correlativo, htmlExterno);
                    //Enviar correo externo
                    List<Emails> emails = await _Queja.obtenerEmailCentralizador(token);
                    if (emails != null)
                    {
                        string[] correoInterno = emails.Select(email => email.Email).ToArray();
                        string htmlInterno = crearHtmlConfirmacionInterno();
                        if (correoInterno.Count() > 0)
                            _Email.enviarCorreo(correoInterno, "Queja Ingresada - " + queja.Correlativo, htmlInterno);
                    }                                        
                    mostrarMensaje("Señor cuentahabiente,  agradecemos su comunicación,  le informamos que su queja ha sido recibida exitosamente. " +
                        "Para el seguimiento o cualquier consulta relacionada, no olvide que el número de su queja es " + queja.Correlativo, 1);
                }                    
                else
                    mostrarMensaje("Hubo un error al almacenar la queja", 3);
                return await IngresoQuejaCliente();
            } catch(Exception ex)
            {
                mostrarMensaje("Hubo un error al guardar la información", 3);
                return await IngresoQuejaCliente();
            }
        }


        private string crearHtmlConfirmacionExterno(string Correlativo)
        {
            string html = "<center> <div style='background: #eeeeee; width:520px; border-radius: 0; padding: 20px;'> <table align='center'> <tbody> <tr> <td colspan='2'> <h3 style='text-align: center;'>Queja Enviada Exitosamente</h3> <p style='text-align: left;'>" +
                            "Señor cuentahabiente,  agradecemos su comunicación,  le informamos que su queja ha sido recibida exitosamente. Para el seguimiento o cualquier consulta relacionada, no olvide que el número de su queja es {0}" +
                            "</p> <center>  ";
            string htmlFinal = string.Format(html,Correlativo);
            return htmlFinal;
        }

        private string crearHtmlConfirmacionInterno()
        {
            string html = "<center> <div style='background: #eeeeee; width:520px; border-radius: 0; padding: 20px;'> <table align='center'> <tbody> <tr> <td colspan='2'> <h3 style='text-align: center;'>Nueva Queja Recibida</h3> <p style='text-align: left;'>" +
                            "El sistema de quejas le informa que se ha recibido una queja, la cual debe ser asignada dentro de las próximas 24 horas." +
                            "</p> <center>  ";            
            return html;
        }


        private async Task llenarTipos()
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<TipoQueja> listaTipos = new List<TipoQueja>();
            if (HttpContext.Cache["listaTipos"] as List<TipoQueja> != null)
                listaTipos = HttpContext.Cache["listaTipos"] as List<TipoQueja>;
            else
            {
                listaTipos = await _TipoQueja.obtenerTiposQueja(token);
                HttpContext.Cache.Insert("listaTipos", listaTipos, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }
            //obtener la lista de regiones
            var tiposSelectList = new List<SelectListItem>(); // Crear una lista vacía de objetos SelectListItem                                
            foreach (var tipo in listaTipos)
            {
                // Agregar cada objeto Region a la lista de objetos SelectListItem                
                tiposSelectList.Add(new SelectListItem { Value = tipo.Id_Tipo.ToString(), Text = tipo.Nombre });
            }
            ViewBag.TiposSelectList = tiposSelectList;
        }
        private void mostrarMensaje(string mensaje, int tipo)
        {
            ViewBag.Mensaje = mensaje;
            ViewBag.Tipo = tipo;
            ViewBag.MostrarModal = true;
        }
    }
}
