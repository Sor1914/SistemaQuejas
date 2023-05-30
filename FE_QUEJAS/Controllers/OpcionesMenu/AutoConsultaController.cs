using FE_QUEJAS.Models;
using FE_QUEJAS.Services;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace FE_QUEJAS.Controllers.OpcionesMenu
{
    public class AutoConsultaController : Controller
    {
        clsTipoQuejaService _TipoQueja = new clsTipoQuejaService();
        clsSeguimientoService _Seguimiento = new clsSeguimientoService();

        string token = "";
        string LayoutMenu = "~/Views/Shared/_LayoutMenu.cshtml";
        // GET: AutoConsulta
        public AutoConsultaController()
        {
            ViewBag.Layout = LayoutMenu;
        }

        public async Task<ActionResult> AutoConsulta(Autoconsulta datos, bool limpiar = true)
        {
            if (limpiar)
            {
                HttpContext.Cache.Remove("listaTipos");
                HttpContext.Cache.Remove("listaRegiones");
            }
            await llenarTipos();
            ViewBag.PaginaActual = ControllerContext.RouteData.Values["action"].ToString();            
            if (datos.Numero_Queja != 0 && datos.Anio_Queja != 0 && datos.Tipo_Queja != null)            
                return PartialView("AutoConsulta", datos);
            else
                return PartialView("AutoConsulta");
        }

        public async Task<ActionResult> buscarQueja(Autoconsulta datos)
        {
            RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();
            if (String.IsNullOrEmpty(recaptchaHelper.Response))
            {
                ModelState.AddModelError("", "Debe llenar el Captcha para continuar");                
                return await AutoConsulta(datos);
            }
            RecaptchaVerificationResult recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();
            if (recaptchaResult.Success != true)
            {
                ModelState.AddModelError("", "Error al validar Captcha");
                return await AutoConsulta(datos);
            }

            await mostrarInformacionQueja(datos);
            return await AutoConsulta(datos);
        }

        private async Task mostrarInformacionQueja(Autoconsulta datos)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            EncabezadoQueja encabezado = new EncabezadoQueja
            {
                Correlativo = datos.Correlativo
            };
            List<EncabezadoQueja> listaEncabezado = await _Seguimiento.obtenerEncabezadoQuejaPorCorrelativo(encabezado, token);
           if(listaEncabezado.Count > 0)
            {
                if (Session["Usuario"].ToString() == listaEncabezado[0].Usuario)
                {
                    ViewBag.Estado = listaEncabezado[0].Id_Estado_Externo;
                    ViewBag.FechaCreacion = listaEncabezado[0].Fecha_Ingreso;
                    ViewBag.MostrarModalQueja = true;
                    ViewBag.TituloModal = "Estado de Queja " + listaEncabezado[0].Correlativo;
                }
                else
                {
                    ModelState.AddModelError("", "Esta queja no existe para el usuario actual.");
                }
            } else
            {
                ModelState.AddModelError("", "Esta queja no existe para el usuario actual.");

            }

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
                tiposSelectList.Add(new SelectListItem { Value = tipo.Siglas_Tipo.ToString(), Text = tipo.Nombre });
            }
            ViewBag.TiposSelectList = tiposSelectList;
        }
    }
}