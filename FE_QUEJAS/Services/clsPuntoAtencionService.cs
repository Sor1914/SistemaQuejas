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
using System.Web.UI.WebControls;

namespace FE_QUEJAS.Services
{
    public class clsPuntoAtencionService
    {
        public async Task<bool> agregarPuntoApi(PuntoAtencion Punto, string token, int opcion)
        {
            bool resultadoIngreso;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);            
            string json = JsonConvert.SerializeObject(Punto);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/PUNTOSATENCION/AGREGARPUNTO", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.Created)
                resultadoIngreso = true;
            else
                resultadoIngreso = false;
            return resultadoIngreso;
        }

        public async Task<bool> editarPuntoApi(PuntoAtencion Punto, string token)
        {
            bool resultadoIngreso;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(Punto);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/PUNTOSATENCION/ACTUALIZARPUNTO", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.OK)
                resultadoIngreso = true;
            else
                resultadoIngreso = false;
            return resultadoIngreso;
        }

        public async Task<bool> eliminarPuntoApi(PuntoAtencion Punto, string token)
        {
            bool resultadoIngreso;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(Punto);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/PUNTOSATENCION/ELIMINARPUNTO", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.OK)
                resultadoIngreso = true;
            else
                resultadoIngreso = false;
            return resultadoIngreso;
        }

        public async Task<List<PuntoAtencion>> obtenerPuntosApi(PuntoAtencion Punto, string token)
        {
            List<PuntoAtencion> objetoRespuesta = new List<PuntoAtencion>();            
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(Punto);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/PUNTOSATENCION/OBTENERPUNTOS", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.Found)            
                objetoRespuesta = JsonConvert.DeserializeObject<List<PuntoAtencion>>(respuestaJson);                        
            return objetoRespuesta;
        }

        public async Task<List<Regiones>> obtenerRegionesApi(string token)
        {
            List<Regiones> objetoRespuesta = new List<Regiones>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await httpClient.GetAsync("http://localhost:61342/API/PUNTOSATENCION/ObtenerRegiones");
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();

            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<Regiones>>(respuestaJson);
            else
                objetoRespuesta = null;
            return objetoRespuesta;
        }
    }
}