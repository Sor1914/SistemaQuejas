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
    public class clsUsuariosService
    {
        public async Task<List<Usuarios>> obtenerUsuarios(string token)
        {
            List<Usuarios> objetoRespuesta = new List<Usuarios>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await httpClient.GetAsync("http://localhost:61342/API/USUARIOS/ObtenerUsuarios");
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.Found)
            {
                objetoRespuesta = JsonConvert.DeserializeObject<List<Usuarios>>(respuestaJson);
            }
            else
                objetoRespuesta = null;
            return objetoRespuesta;
        }

        public async Task<List<Roles>> obtenerRoles(string token)
        {
            List<Roles> objetoRespuesta = new List<Roles>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respuesta = await httpClient.GetAsync("http://localhost:61342/API/USUARIOS/ObtenerRoles");
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode == HttpStatusCode.Found)
            {
                objetoRespuesta = JsonConvert.DeserializeObject<List<Roles>>(respuestaJson);
            }
            else
                objetoRespuesta = null;
            return objetoRespuesta;
        }

        public async Task<bool> actualizarUsuario(Usuarios usuario, string token)
        {
            List<Usuarios> objetoRespuesta = new List<Usuarios>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(usuario);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/USUARIOS/ActualizarUsuario", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;            
        }

        public async Task<bool> eliminarUsuario(Usuarios usuario, string token)
        {
            List<Usuarios> objetoRespuesta = new List<Usuarios>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string json = JsonConvert.SerializeObject(usuario);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/USUARIOS/EliminarUsuario", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;            
        }
    }
}