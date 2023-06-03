using FE_QUEJAS.Models;
using FE_QUEJAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Web.Caching;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace FE_QUEJAS.Controllers.OpcionesMenu
{
    public class ReportesController : Controller
    {

        string token = "";
        string LayoutMenu = "~/Views/Shared/_LayoutMenu.cshtml";
        clsReporteService _Consultas = new clsReporteService();
        clsUsuarioPuntoAtencionService _Usuario = new clsUsuarioPuntoAtencionService();

        // GET: Reportes
        public ReportesController()
        {
            ViewBag.Layout = LayoutMenu;
        }

        public async Task<ActionResult> Reportes(bool limpiar = true)
        {
            if (limpiar)
            {
                HttpContext.Cache.Remove("listaRegiones");
            }            
            ViewBag.FechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.PaginaActual = ControllerContext.RouteData.Values["action"].ToString();
            await llenarRegiones(0);
            await llenarPuntos();
            return PartialView("Reportes");
        }


        public async Task<ActionResult> BuscarRegistros(Reporte reporte)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<EncabezadoQuejaReporte> listaEncabezado = await _Consultas.obtenerQuejas(reporte, token);
            HttpContext.Cache.Insert("listaQuejas", listaEncabezado, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            return await Paginacion(1,5,true);
        }
  
        public async Task<ActionResult> CrearReporte(string tipoDescarga)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<EncabezadoQuejaReporte> listaEncabezado = HttpContext.Cache["listaQuejas"] as List<EncabezadoQuejaReporte>;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("REPORTE DE QUEJAS");
                var titleCell = worksheet.Cell(1, 1);
                titleCell.Value = "REPORTE DE QUEJAS POR MAL SERVICIO O SERVICIO NO CONFORME";
                titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleCell.Style.Font.Bold = true;
                var lastColumn = worksheet.Column(worksheet.ColumnCount());
                var titleRange = worksheet.Range(worksheet.Cell(1,1), worksheet.Cell(1,8));
                titleRange.Merge();
                worksheet.Columns().AdjustToContents();
                worksheet.Cell(2, 1).Value = "No. Queja";
                worksheet.Cell(2, 2).Value = "Tipo de Queja";
                worksheet.Cell(2, 3).Value = "Punto de Atención";
                worksheet.Cell(2, 4).Value = "Estado";
                worksheet.Cell(2, 5).Value = "Etapa";
                worksheet.Cell(2, 6).Value = "Resultado";
                worksheet.Cell(2, 7).Value = "Medio de Ingreso";
                worksheet.Cell(2, 8).Value = "Fecha de Creación";
                worksheet.Cell(2, 9).Value = "Detalle";
                var range1 = worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(2, 9)); // Rango de 8 columnas
                range1.Style.Fill.BackgroundColor = XLColor.Black;
                range1.Style.Font.FontColor = XLColor.White;
                bool gris = false;
                for (int i = 0; i < listaEncabezado.Count; i++)
                {
                    worksheet.Cell(i + 3, 1).Value = listaEncabezado[i].Correlativo;
                    worksheet.Cell(i + 3, 2).Value = listaEncabezado[i].Nombre_Tipo_Queja;
                    worksheet.Cell(i + 3, 3).Value = listaEncabezado[i].Nombre_Punto_Atencion;
                    worksheet.Cell(i + 3, 4).Value = listaEncabezado[i].Estado_Interno;
                    worksheet.Cell(i + 3, 5).Value = listaEncabezado[i].Estado_Externo;
                    worksheet.Cell(i + 3, 6).Value = listaEncabezado[i].Justificacion;
                    worksheet.Cell(i + 3, 7).Value = listaEncabezado[i].Nombre_Origen;
                    worksheet.Cell(i + 3, 8).Value = listaEncabezado[i].Fecha_Ingreso;
                    worksheet.Cell(i + 3, 9).Value = listaEncabezado[i].Detalle;
                    var range = worksheet.Range(worksheet.Cell(i + 3, 1), worksheet.Cell(i + 3, 9)); // Rango de 8 columnas
                    if (gris)
                    {
                        range.Style.Fill.BackgroundColor = XLColor.Gray;
                        range.Style.Font.FontColor = XLColor.White;
                    }                                      
                    gris = !gris;
                }
                worksheet.Columns().AdjustToContents();                                
                var tempFilePath = Path.GetTempFileName() + ".xlsx";
                workbook.SaveAs(tempFilePath);
                if (tipoDescarga == "PDF")
                {
                    var tempPdfFilePath = Path.GetTempFileName() + ".pdf";
                    ConvertXlsxToPdf(tempFilePath, tempPdfFilePath);
                    return File(tempPdfFilePath, "application/pdf", "reporte.pdf");
                } else
                {
                    return File(tempFilePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporte.xlsx");
                }                                
            }

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

        public async Task<ActionResult> Paginacion(int pagina = 1, int tamanoPagina = 5, bool primeraVez = false)
        {
            token = Request.Cookies["TokenJwt"]?.Value;
            List<EncabezadoQuejaReporte> ListaQuejas = new List<EncabezadoQuejaReporte>();
            List<EncabezadoQuejaReporte> ListaQuejasFiltro = new List<EncabezadoQuejaReporte>();
            await llenarPuntos();
            await llenarRegiones(0);            
            ListaQuejas = HttpContext.Cache["listaQuejas"] as List<EncabezadoQuejaReporte>;            
            ListaQuejasFiltro = ListaQuejas;
            int totalElementos = ListaQuejasFiltro.Count;
            var elementosPagina = ListaQuejasFiltro.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina);
            ViewBag.PaginaActualTabla = pagina;
            ViewBag.TamanoPagina = tamanoPagina;
            ViewBag.TotalElementos = totalElementos;
            ViewBag.TotalPaginas = (int)Math.Ceiling((decimal)totalElementos / tamanoPagina);
            ViewBag.ListaQuejas = elementosPagina;
            if (primeraVez)
            {
                ViewBag.Reportes = "True";
                return await Reportes();                
            }
            return PartialView("_TablaReportes");
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
        

        private void ConvertXlsxToPdf(string sourcePath, string destinationPath)
        {
            using (var document = new Document())
            {
                var writer = PdfWriter.GetInstance(document, new FileStream(destinationPath, FileMode.Create));
                document.Open();

                using (var stream = new FileStream(sourcePath, FileMode.Open))
                {
                    using (var workbook = new XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RangeUsed().RowsUsed();
                        bool primero = true;
                        foreach (var row in rows)
                        {
                            if (primero)
                            {
                                primero = !primero;
                                continue;
                            }
                            var pdfTable = new PdfPTable(worksheet.Columns().Count());

                            foreach (var cell in row.Cells())
                            {
                                var cellValue = cell.Value?.ToString();
                                var pdfCell = new PdfPCell(new Phrase(cellValue));
                                pdfTable.AddCell(pdfCell);
                            }

                            document.Add(pdfTable);
                        }
                    }
                }

                document.Close();
            }
        }



    }
}