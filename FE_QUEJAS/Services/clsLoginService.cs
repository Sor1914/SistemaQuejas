using FE_QUEJAS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FE_QUEJAS.Services
{
    public class clsLoginService
    {
        
        public async Task<string> iniciarSesionApi(LoginRequest Login)
        {
            string token = "NE";
            HttpClient httpClient = new HttpClient();            
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = JsonConvert.SerializeObject(Login);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/LOGIN/AUTENTICAR", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                token = resultado.ToString();
            }
            else
            {
                token = "Error";
            }
            return token;
        }
    }
}
