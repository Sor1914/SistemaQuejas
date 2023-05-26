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
    public class clsSeguimientoService
    {
        public async Task<List<EncabezadoQueja>> obtenerQuejasAsignacion(string token)
        {
            List<EncabezadoQueja> objetoRespuesta = new List<EncabezadoQueja>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await httpClient.GetAsync("http://localhost:61342/API/SEGUIMIENTO/ObtenerQuejasAsignacion");
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<EncabezadoQueja>>(respuestaJson);
            return objetoRespuesta;
        }

        public async Task<List<EncabezadoQueja>> obtenerQuejasPA(string token)
        {
            List<EncabezadoQueja> objetoRespuesta = new List<EncabezadoQueja>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await httpClient.GetAsync("http://localhost:61342/API/SEGUIMIENTO/ObtenerQuejasPA");
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.Found)        
                objetoRespuesta = JsonConvert.DeserializeObject<List<EncabezadoQueja>>(respuestaJson);        
            return objetoRespuesta;
        }

        public async Task<List<EncabezadoQueja>> obtenerQuejasCent(string token)
        {
            List<EncabezadoQueja> objetoRespuesta = new List<EncabezadoQueja>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await httpClient.GetAsync("http://localhost:61342/API/SEGUIMIENTO/ObtenerQuejasCent");
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<EncabezadoQueja>>(respuestaJson);
            return objetoRespuesta;
        }

        public async Task<List<EncabezadoQueja>> obtenerEncabezadoQueja(EncabezadoQueja encabezado, string token)
        {
            List<EncabezadoQueja> objetoRespuesta = new List<EncabezadoQueja>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(encabezado);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/SEGUIMIENTO/ObtenerEncabezadoQueja", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<EncabezadoQueja>>(respuestaJson);
            return objetoRespuesta;
        }

        public async Task<List<EncabezadoQueja>> obtenerEncabezadoQuejaPorCorrelativo(EncabezadoQueja encabezado, string token)
        {
            List<EncabezadoQueja> objetoRespuesta = new List<EncabezadoQueja>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(encabezado);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/SEGUIMIENTO/ObtenerEncabezadoQuejaPorCorrelativo", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<EncabezadoQueja>>(respuestaJson);
            return objetoRespuesta;
        }

        public async Task<List<DetalleQueja>> obtenerDetalleQueja(DetalleQueja detalle, string token)
        {
            List<DetalleQueja> objetoRespuesta = new List<DetalleQueja>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(detalle);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/SEGUIMIENTO/ObtenerDetalleQueja", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<DetalleQueja>>(respuestaJson);
            return objetoRespuesta;
        }

        public async Task<DetalleQueja> agregarDetalleQueja(DetalleQueja detalle, string token)
        {
            DetalleQueja resultadoIngreso;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(detalle.Comentario), "Comentario");
                    formData.Add(new StringContent(detalle.Id_Encabezado.ToString()), "Id_Encabezado");
                    if (detalle.ArchivoAdjunto != null)
                    {
                        StreamContent archivoAdjuntoContent = new StreamContent(detalle.ArchivoAdjunto.InputStream);
                        archivoAdjuntoContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "ArchivoAdjunto",
                            FileName = detalle.ArchivoAdjunto.FileName,
                        };
                        formData.Add(archivoAdjuntoContent);
                    }
                    HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/SEGUIMIENTO/InsertarDetalleQueja", formData);
                    string respuestaJson = await respuesta.Content.ReadAsStringAsync();
                    if (respuesta.StatusCode == HttpStatusCode.OK)
                        resultadoIngreso = JsonConvert.DeserializeObject<DetalleQueja>(respuestaJson);
                    else
                        resultadoIngreso = null;
                }
            }

            return resultadoIngreso;
        }

        public async Task<List<EncabezadoQueja>> actualizarPuntoEstadoQueja(EncabezadoQueja encabezado, string token)
        {
            List<EncabezadoQueja> objetoRespuesta = new List<EncabezadoQueja>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(encabezado);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/SEGUIMIENTO/ActualizarPuntoEstadoQueja", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.OK)
                objetoRespuesta = JsonConvert.DeserializeObject<List<EncabezadoQueja>>(respuestaJson);
            else
                objetoRespuesta = null;
            return objetoRespuesta;
        }

        public async Task<List<EncabezadoQueja>> actualizarEstadoQueja(EncabezadoQueja encabezado, string token)
        {
            List<EncabezadoQueja> objetoRespuesta = new List<EncabezadoQueja>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(encabezado);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/SEGUIMIENTO/ActualizarEstadoQueja", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.OK)
                objetoRespuesta = JsonConvert.DeserializeObject<List<EncabezadoQueja>>(respuestaJson);
            else
                objetoRespuesta = null;
            return objetoRespuesta;
        }

        public async Task<List<EncabezadoQueja>> obtenerEmailsPunto(EncabezadoQueja encabezado, string token)
        {
            List<EncabezadoQueja> objetoRespuesta = new List<EncabezadoQueja>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(encabezado);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/SEGUIMIENTO/ObtenerEmailsPuntoAtencion", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<EncabezadoQueja>>(respuestaJson);
            return objetoRespuesta;
        }

    }
}