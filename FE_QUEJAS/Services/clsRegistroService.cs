﻿using FE_QUEJAS.Models;
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
        public async Task<bool> registrarseApi(RegistroRequest registro)
        {
            bool registrado;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = JsonConvert.SerializeObject(registro);
            HttpResponseMessage respuesta = await httpClient.PostAsync("http://localhost:61342/API/LOGIN/REGISTRAR", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            string respuestaJson = await respuesta.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
            if (respuesta.StatusCode == HttpStatusCode.OK)            
                registrado = true;        
            else            
                registrado = false;            
            return registrado;
        }
    }
}
