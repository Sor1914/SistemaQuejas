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
    public class clsQuejaService
    {
        public async Task<Queja> agregarQuejaApiEspera(Queja queja, string token)
        {
            Queja resultadoIngreso;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(queja);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://20.92.250.90/API_Quejas/API/QUEJA/InsertarTipoQueja", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.OK)
                resultadoIngreso = JsonConvert.DeserializeObject<Queja>(respuestaJson);
            else
                resultadoIngreso = null;
            return resultadoIngreso;
        }

        public async Task<Queja> agregarQuejaApi(Queja queja, string token)
        {
            Queja resultadoIngreso;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using (var formData = new MultipartFormDataContent())
                {                    
                    formData.Add(new StringContent(queja.Nombres), "Nombres");
                    formData.Add(new StringContent(queja.Apellidos), "Apellidos");
                    formData.Add(new StringContent(queja.Email), "Email");
                    formData.Add(new StringContent(queja.Telefono), "Telefono");
                    formData.Add(new StringContent(queja.Detalle), "Detalle");
                    formData.Add(new StringContent(queja.Tipo_Queja.ToString()), "Tipo_Queja");                   
                    if (queja.ArchivoAdjunto != null)
                    {
                        StreamContent archivoAdjuntoContent = new StreamContent(queja.ArchivoAdjunto.InputStream);
                        archivoAdjuntoContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "ArchivoAdjunto",
                            FileName = queja.ArchivoAdjunto.FileName,
                        };
                        formData.Add(archivoAdjuntoContent);
                    }                                         
                    HttpResponseMessage respuesta = await httpClient.PostAsync("http://20.92.250.90/API_Quejas/API/QUEJA/InsertarTipoQueja", formData);
                    string respuestaJson = await respuesta.Content.ReadAsStringAsync();
                    if (respuesta.StatusCode == HttpStatusCode.OK)
                        resultadoIngreso = JsonConvert.DeserializeObject<Queja>(respuestaJson);
                    else
                        resultadoIngreso = null;
                }
            }

            return resultadoIngreso;
        }

        public async Task<List<Emails>> obtenerEmailCentralizador(string token)
        {
            List<Emails> objetoRespuesta = new List<Emails>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await httpClient.GetAsync("http://20.92.250.90/API_Quejas/API/QUEJA/ObtenerCorreoCentralizador");
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<Emails>>(respuestaJson);
            else
                objetoRespuesta = null;
            return objetoRespuesta;
        }

    }



}