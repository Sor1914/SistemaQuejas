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
    public class clsTipoQuejaService
    {
        public async Task<List<TipoQueja>> obtenerTiposQueja(string token)
        {
            List<TipoQueja> objetoRespuesta = new List<TipoQueja>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await httpClient.GetAsync("http://20.92.250.90/API_Quejas/API/TIPOQUEJA/ObtenerTiposQueja");
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<TipoQueja>>(respuestaJson);
            else
                objetoRespuesta = null;
            return objetoRespuesta;
        }
        public async Task<bool> insertarTipoQuejaApi(TipoQueja queja, string token)
        {
            bool resultadoIngreso;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(queja);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://20.92.250.90/API_Quejas/API/TIPOQUEJA/InsertarTipoQueja", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.OK)
                resultadoIngreso = true;
            else
                resultadoIngreso = false;
            return resultadoIngreso;
        }
        public async Task<bool> actualizarTipoQuejaApi(TipoQueja queja, string token)
        {
            bool resultadoIngreso;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(queja);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://20.92.250.90/API_Quejas/API/TIPOQUEJA/ActualizarTipoQueja", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.OK)
                resultadoIngreso = true;
            else
                resultadoIngreso = false;
            return resultadoIngreso;
        }

        public async Task<bool> eliminarTipoQueja(TipoQueja queja, string token)
        {
            bool resultadoIngreso;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(queja);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://20.92.250.90/API_Quejas/API/TIPOQUEJA/EliminarTipoQueja", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.OK)
                resultadoIngreso = true;
            else
                resultadoIngreso = false;
            return resultadoIngreso;
        }
    }
}