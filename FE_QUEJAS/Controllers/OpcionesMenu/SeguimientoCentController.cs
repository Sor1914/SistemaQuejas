using FE_QUEJAS.Models;
using FE_QUEJAS.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace FE_QUEJAS.Controllers.OpcionesMenu
{
    public class SeguimientoCentController : Controller
    {
        string token = "";
        string LayoutMenu = "~/Views/Shared/_LayoutMenu.cshtml";
        clsSeguimientoService _Asignacion = new clsSeguimientoService();
        clsUsuarioPuntoAtencionService _Usuario = new clsUsuarioPuntoAtencionService();
        clsEnvioEmail _Email = new clsEnvioEmail();

        public SeguimientoCentController()
        {
            ViewBag.Layout = LayoutMenu;
        }
        public async Task<ActionResult> SeguimientoCent(bool limpiar = true)
        {
            if (limpiar)
            {
                HttpContext.Cache.Remove("listaQuejas");
                HttpContext.Cache.Remove("listaRegiones");
            }
            ViewBag.PaginaActual = ControllerContext.RouteData.Values["action"].ToString();
            await Paginacion();
            return PartialView("SeguimientoCent");
        }

        private async Task llenarRegiones(int idRegion)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<Regiones> listaRegiones = new List<Regiones>();
            if (HttpContext.Cache["listaRegiones"] as List<Regiones> != null)
                listaRegiones = HttpContext.Cache["listaRegiones"] as List<Regiones>;
            else
            {
                listaRegiones = await _Usuario.obtenerRegionesApi(token);
                HttpContext.Cache.Insert("listaRegiones", listaRegiones, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }
            //obtener la lista de regiones
            var regionesSelectList = new List<SelectListItem>(); // Crear una lista vacía de objetos SelectListItem                                
            foreach (var region in listaRegiones)
            {
                // Agregar cada objeto Region a la lista de objetos SelectListItem
                if (idRegion == region.Id_Region)
                    regionesSelectList.Add(new SelectListItem { Value = region.Id_Region.ToString(), Text = region.Nombre_Region, Selected = true });
                else
                    regionesSelectList.Add(new SelectListItem { Value = region.Id_Region.ToString(), Text = region.Nombre_Region });
            }
            ViewBag.RegionesSelectList = regionesSelectList;
        }

        public async Task<ActionResult> Paginacion(int pagina = 1, int tamanoPagina = 5)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<EncabezadoQueja> ListaQuejas = new List<EncabezadoQueja>();
            List<EncabezadoQueja> ListaQuejasFiltro = new List<EncabezadoQueja>();
            await llenarPuntos();
            await llenarRegiones(0);
            if (HttpContext.Cache["listaQuejas"] != null)
                ListaQuejas = HttpContext.Cache["listaQuejas"] as List<EncabezadoQueja>;
            else
            {
                ListaQuejas = await _Asignacion.obtenerQuejasCent(token);
                HttpContext.Cache.Insert("listaQuejas", ListaQuejas, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }
            ListaQuejasFiltro = ListaQuejas;
            int totalElementos = ListaQuejasFiltro.Count;
            var elementosPagina = ListaQuejasFiltro.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina);
            ViewBag.PaginaActualTabla = pagina;
            ViewBag.TamanoPagina = tamanoPagina;
            ViewBag.TotalElementos = totalElementos;
            ViewBag.TotalPaginas = (int)Math.Ceiling((decimal)totalElementos / tamanoPagina);
            ViewBag.ListaQuejas = elementosPagina;
            return PartialView("_TablaQuejasPA");
        }

        public async Task<ActionResult> mostrarModalVer(int idEncabezado)
        {
            token = Request.Cookies["TokenJwt"]?.Value;

            EncabezadoQueja encabezado = new EncabezadoQueja
            {
                Id_Encabezado = idEncabezado
            };
            DetalleQueja detalle = new DetalleQueja
            {
                Id_Encabezado = idEncabezado
            };
            await AsignarValoresEncabezado(encabezado, token);
            await asignarValoresDetalle(detalle, token);
            ViewBag.MostrarModalVer = true;
            return await SeguimientoCent(false);
        }

        public async Task<ActionResult> mostrarModalDetalle(int idEncabezado)
        {
            ViewBag.MostrarModalDetalle = true;
            ViewBag.IdEncabezado = idEncabezado;
            return await SeguimientoCent(false);
        }



        public async Task<ActionResult> mostrarModalAsignar(int idEncabezado)
        {
            ViewBag.MostrarModalAsignar = true;
            ViewBag.IdEncabezado = idEncabezado;
            return await SeguimientoCent(false);
        }

        public async Task<ActionResult> mostrarModalResolver(int idEncabezado)
        {
            ViewBag.MostrarModalResolver = true;
            ViewBag.IdEncabezado = idEncabezado;
            return await SeguimientoCent(false);
        }

        public async Task<ActionResult> guardarAsignacionQueja(EncabezadoQueja encabezado, int idEncabezado)
        {
            try
            {
                List<EncabezadoQueja> listEncabezado = new List<EncabezadoQueja>();
                List<EncabezadoQueja> listEmail = new List<EncabezadoQueja>();
                encabezado.Id_Encabezado = idEncabezado;
                encabezado.Id_Estado_Externo = 4;
                encabezado.Id_Estado_Interno = 2;
                token = Request.Cookies["TokenJwt"]?.Value;
                listEncabezado = await _Asignacion.actualizarPuntoEstadoQueja(encabezado, token);
                if (encabezado != null)
                {
                    string htmlInterno = crearHtmlAsignacionInterno(listEncabezado[0].Correlativo);
                    listEmail = await _Asignacion.obtenerEmailsPunto(encabezado, token);
                    string[] correosInternos = listEmail.Select(Email => Email.Email.ToString()).ToArray();
                    if (correosInternos.Count() > 0)
                        _Email.enviarCorreo(correosInternos, "Queja Asignada - " + encabezado.Correlativo, htmlInterno);
                    mostrarMensaje("La información se almacenó correctamente", 1);
                }
                else
                    mostrarMensaje("Hubo un error al guardar la información", 3);
                return await SeguimientoCent(true);
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al guardar la información", 3);
                return await SeguimientoCent();
            }
        }



        public async Task<ActionResult> resolverQueja(EncabezadoQueja encabezado, int idEncabezado)
        {
            try
            {
                encabezado.Id_Encabezado = idEncabezado;
                encabezado.Id_Estado_Externo = 9;
                encabezado.Id_Estado_Interno = 9;
                token = Request.Cookies["TokenJwt"]?.Value;
                List<EncabezadoQueja> encabezadoQuejaOk = await _Asignacion.actualizarEstadoQueja(encabezado, token);
                if (encabezadoQuejaOk != null)
                {
                    string html = crearHtmlFinalizacion(encabezado.Justificacion);
                    string[] correo = new string[1];
                    correo[0] = encabezadoQuejaOk[0].Email;
                    if (correo[0] != null || correo[0] != "")
                        _Email.enviarCorreo(correo, "Queja Finalizada - " + encabezadoQuejaOk[0].Correlativo, html);
                    string mensajeNotificacion = "La queja se actualizó correctamente.";
                    mostrarMensaje(mensajeNotificacion, 1);
                }
                else
                    mostrarMensaje("Hubo un error al guardar la información", 3);
                return await SeguimientoCent(true);
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al guardar la información", 3);
                return await SeguimientoCent();
            }
        }

        public async Task<ActionResult> mostrarModalRechazo(int idEncabezado)
        {
            ViewBag.mostrarModalRechazo = true;
            ViewBag.IdEncabezado = idEncabezado;
            return await SeguimientoCent(false);
        }

        public async Task<ActionResult> guardarRechazoQueja(EncabezadoQueja encabezado, int idEncabezado)
        {
            try
            {
                List<EncabezadoQueja> listEncabezado = new List<EncabezadoQueja>();
                List<EncabezadoQueja> listEmail = new List<EncabezadoQueja>();
                encabezado.Id_Encabezado = idEncabezado;
                encabezado.Id_Estado_Externo = 4;
                encabezado.Id_Estado_Interno = 8;
                token = Request.Cookies["TokenJwt"]?.Value;
                string mensajeNotificacion = "";
                List<EncabezadoQueja> encabezadoQuejaOk = await _Asignacion.actualizarEstadoQueja(encabezado, token);
                mensajeNotificacion = "La información se almacenó correctamente";
                if (encabezadoQuejaOk != null)
                {
                    string htmlInterno = crearHtmlReAsignacionInterno(encabezadoQuejaOk[0].Correlativo);
                    listEmail = await _Asignacion.obtenerEmailsPunto(encabezado, token);
                    string[] correosInternos = listEmail.Select(Email => Email.Email.ToString()).ToArray();
                    if (correosInternos.Count() > 0)
                        _Email.enviarCorreo(correosInternos, "Queja Reasignada - " + encabezado.Correlativo, htmlInterno);
                    mostrarMensaje("La información se almacenó correctamente", 1);
                }
                else
                    mostrarMensaje("Hubo un error al guardar la información", 3);
                return await SeguimientoCent(true);
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al guardar la información", 3);
                return await SeguimientoCent();
            }
        }

        private string crearHtmlFinalizacion(string justificacion)
        {
            string html = "<center> <div style='background: #eeeeee; width:520px; border-radius: 0; padding: 20px;'> <table align='center'> <tbody> <tr> <td colspan='2'> <h3 style='text-align: center;'>Queja Finalizada</h3> <p style='text-align: left;'>" +
                            "Señor cuentahabiente, la atención de su queja fue finalizada con el resultado siguiente: {0}" +
                            "</p> <center>  ";
            string htmlFinal = string.Format(html, justificacion);
            return htmlFinal;
        }

        private string crearHtmlReAsignacionInterno(string queja)
        {
            string html = "<center> <div style='background: #eeeeee; width:520px; border-radius: 0; padding: 20px;'> <table align='center'> <tbody> <tr> <td colspan='2'> <h3 style='text-align: center;'>Queja Reasignada</h3> <p style='text-align: left;'>" +
                            "Estimados, El sistema para control de quejas por mal servicio o servicio no conforme le informa que se le asignó la queja No. [{0}] para su reanálisis” Para su atención tiene un plazo máximo de 24 horas, según normativa vigente." +
                            "</p> <center>  ";
            string htmlFinal = string.Format(html, queja);
            return htmlFinal;
        }

        private string crearHtmlAsignacionInterno(string queja)
        {
            string html = "<center> <div style='background: #eeeeee; width:520px; border-radius: 0; padding: 20px;'> <table align='center'> <tbody> <tr> <td colspan='2'> <h3 style='text-align: center;'>Queja Reasignada</h3> <p style='text-align: left;'>" +
                            "Estimados, El sistema para control de quejas por mal servicio o servicio no conforme le informa que se le asignó la queja No. [{0}. Para su atención tiene un plazo máximo de 5 días hábiles, según normativa vigente.]" +
                            "</p> <center>  ";
            string htmlFinal = string.Format(html, queja);
            return htmlFinal;
        }

        private string crearHtmlAsignacionExterno(string queja)
        {
            string html = "<center> <div style='background: #eeeeee; width:520px; border-radius: 0; padding: 20px;'> <table align='center'> <tbody> <tr> <td colspan='2'> <h3 style='text-align: center;'>Queja Asignada</h3> <p style='text-align: left;'>" +
                            "Señor(a) Cuentahabiente, su queja ha sido trasladada al administrador del punto de atención correspondiente para su análisis, {0}" +
                            "</p> <center>  ";
            string htmlFinal = string.Format(html, queja);
            return htmlFinal;
        }

        private async Task llenarPuntos()
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<PuntoAtencion> listaPuntos = new List<PuntoAtencion>();
            List<PuntoAtencion> listaPuntosFiltro = new List<PuntoAtencion>();
            var puntosSelectList = new List<SelectListItem>(); // Crear una lista vacía de objetos SelectListItem
            if (HttpContext.Cache["listaPuntos"] != null)
                listaPuntos = HttpContext.Cache["listaPuntos"] as List<PuntoAtencion>;
            else
            {
                listaPuntos = await _Usuario.obtenerPuntosApi(token);
                HttpContext.Cache.Insert("listaPuntos", listaPuntos, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }
        }

        public JsonResult ObtenerPuntosDeRegion(int idRegion = 0)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<PuntoAtencion> listaPuntos = new List<PuntoAtencion>();
            List<PuntoAtencion> listaPuntosFiltro = new List<PuntoAtencion>();
            var puntosSelectList = new List<SelectListItem>(); // Crear una lista vacía de objetos SelectListItem
            if (idRegion != 0)
            {
                listaPuntos = HttpContext.Cache["listaPuntos"] as List<PuntoAtencion>;
                //Filtrar por región                
                listaPuntosFiltro = listaPuntos.Where(p => p.IdRegion == idRegion).ToList();
                //obtener la lista de regiones                
                foreach (var punto in listaPuntosFiltro)
                {
                    // Agregar cada objeto Region a la lista de objetos SelectListItem                    
                    puntosSelectList.Add(new SelectListItem { Value = punto.Id.ToString(), Text = punto.NombrePuntoAtencion });
                }
            }
            var puntos = puntosSelectList;
            return Json(puntos, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> guardarDetalle(DetalleQueja detalle, int idEncabezado)
        {
            try
            {
                token = Request.Cookies["TokenJwt"]?.Value;
                detalle.Id_Encabezado = idEncabezado;
                HttpPostedFileBase archivoAdjunto = Request.Files["ArchivoAdjunto"];
                if (archivoAdjunto != null && archivoAdjunto.ContentLength > 0)
                {
                    detalle.ArchivoAdjunto = archivoAdjunto;
                }
                detalle = await _Asignacion.agregarDetalleQueja(detalle, token);
                if (detalle != null)
                {
                    mostrarMensaje("El detalle se almacenó correctamente", 1);
                }
                else
                {
                    mostrarMensaje("Hubo un error al almacenar el detalle", 3);
                }
                return await SeguimientoCent(false);
            }
            catch (Exception ex)
            {
                mostrarMensaje("Hubo un error al guardar el detalle", 3);
                return await SeguimientoCent();
            }
        }
        public async Task AsignarValoresEncabezado(EncabezadoQueja encabezado, string token)
        {
            List<EncabezadoQueja> listaEncabezado = await _Asignacion.obtenerEncabezadoQueja(encabezado, token);
            encabezado = listaEncabezado[0];
            ViewBag.Nombre = encabezado.Nombres + " " + encabezado.Apellidos;
            ViewBag.Email = encabezado.Email;
            ViewBag.Telefono = encabezado.Telefono;
            ViewBag.Etapa = encabezado.Estado_Interno;
            ViewBag.Region = encabezado.Nombre_Region;
            ViewBag.PuntoAtencion = encabezado.Nombre_Punto_Atencion;
            ViewBag.Queja = encabezado.Detalle;
            ViewBag.Url = encabezado.Direccion_Archivo;
            ViewBag.Correlativo = encabezado.Correlativo;
        }

        public async Task asignarValoresDetalle(DetalleQueja detalle, string token)
        {
            List<DetalleQueja> listaDetalle = await _Asignacion.obtenerDetalleQueja(detalle, token);
            ViewBag.ListaDetalle = listaDetalle;
        }

        private void mostrarMensaje(string mensaje, int tipo)
        {
            ViewBag.Mensaje = mensaje;
            ViewBag.Tipo = tipo;
            ViewBag.MostrarModal = true;
        }


        public async Task<ActionResult> DescargarArchivoAdjunto(string urlArchivo)
        {
            try
            {
                token = Request.Cookies["TokenJwt"]?.Value;

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var contenido = new StringContent("", Encoding.UTF8, "application/json");
                    HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/SEGUIMIENTO/DescargarArchivo?direccionArchivo=" + urlArchivo, contenido);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        var archivoBytes = await respuesta.Content.ReadAsByteArrayAsync();
                        var nombreArchivo = respuesta.Content.Headers.ContentDisposition.FileName;

                        // Devuelve el archivo para descarga
                        return File(archivoBytes, "application/octet-stream", nombreArchivo);
                    }
                    else
                    {
                        string mensajeError = await respuesta.Content.ReadAsStringAsync();
                        return Content(mensajeError);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return Content(mensaje);
            }
        }
    }
}