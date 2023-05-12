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
    public class clsUsuarioPuntoAtencionService
    {
        public async Task<List<Regiones>> obtenerRegionesApi(string token)
        {
            List<Regiones> objetoRespuesta = new List<Regiones>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await httpClient.GetAsync("http://localhost:61342/API/USUARIOPUNTOATENCION/ObtenerRegiones");
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();

            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<Regiones>>(respuestaJson);
            else
                objetoRespuesta = null;
            return objetoRespuesta;
        }

        public async Task<List<Cargos>> obtenerCargosApi(string token)
        {
            List<Cargos> objetoRespuesta = new List<Cargos>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await httpClient.GetAsync("http://localhost:61342/API/USUARIOPUNTOATENCION/ObtenerCargos");
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();

            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<Cargos>>(respuestaJson);
            else
                objetoRespuesta = null;
            return objetoRespuesta;
        }
        public async Task<List<PuntoAtencion>> obtenerPuntosApi(string token)
        {
            List<PuntoAtencion> objetoRespuesta = new List<PuntoAtencion>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject("");
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/USUARIOPUNTOATENCION/ObtenerPuntos", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<PuntoAtencion>>(respuestaJson);
            return objetoRespuesta;
        }

        public async Task<List<UsuarioPuntoAtencion>> obtenerusuariosApi(string token)
        {
            List<UsuarioPuntoAtencion> objetoRespuesta = new List<UsuarioPuntoAtencion>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject("");
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/USUARIOPUNTOATENCION/ObtenerUsuarios", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<UsuarioPuntoAtencion>>(respuestaJson);
            return objetoRespuesta;
        }

        public async Task<bool> actualizarDatosUsuario(UsuarioPuntoAtencion usuario, string token)
        {
            bool resultadoIngreso;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(usuario);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/USUARIOPUNTOATENCION/ActualizarDatosUsuario", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.OK)
                resultadoIngreso = true;
            else
                resultadoIngreso = false;
            return resultadoIngreso;
        }

        public async Task<List<UsuarioPuntoAtencion>> obtenerUsuarioPorCui(string token, UsuarioPuntoAtencion usuario)
        {
            List<UsuarioPuntoAtencion> objetoRespuesta = new List<UsuarioPuntoAtencion>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(usuario);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/USUARIOPUNTOATENCION/ObtieneDatosUsuarioPorCui", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.Found)
                objetoRespuesta = JsonConvert.DeserializeObject<List<UsuarioPuntoAtencion>>(respuestaJson);
            return objetoRespuesta;
        }
    }
}