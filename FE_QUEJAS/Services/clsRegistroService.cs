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
    public class clsRegistroService
    {                
        public async Task<string> registrarseApi(RegistroRequest registro)
        {
            string registrado;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = JsonConvert.SerializeObject(registro);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://20.92.250.90/API_Quejas/API/LOGIN/REGISTRAR", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();                        
            if (respuesta.StatusCode == HttpStatusCode.OK)
                registrado = "Ok";
            else
            {
                Mensaje resultado = JsonConvert.DeserializeObject<Mensaje>(respuestaJson);
                registrado = resultado.Message;
            }                
            return registrado;
        }
    }
}
