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
        
        public async Task<LoginRequest> iniciarSesionApi(LoginRequest Login)
        {
            Login.Token = "NE";
            HttpClient httpClient = new HttpClient();            
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = JsonConvert.SerializeObject(Login);
            //http://20.92.250.90/API_Quejas/
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://20.92.250.90/API_Quejas/API/LOGIN/AUTENTICAR", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                Login = JsonConvert.DeserializeObject<LoginRequest>(respuestaJson);
            }
            return Login;
        }      
    }
}
