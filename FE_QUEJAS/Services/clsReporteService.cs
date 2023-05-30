using FE_QUEJAS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace FE_QUEJAS.Services
{
    public class clsReporteService
    {
        public async Task<List<EncabezadoQuejaReporte>> obtenerQuejas(Reporte reporte, string token)
        {
            List<EncabezadoQuejaReporte> objetoRespuesta = new List<EncabezadoQuejaReporte>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(reporte);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://20.92.250.90/API_Quejas/API/Reportes/ObtenerQuejasReporte", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.OK)
                objetoRespuesta = JsonConvert.DeserializeObject<List<EncabezadoQuejaReporte>>(respuestaJson);
            return objetoRespuesta;
        }
    }
}