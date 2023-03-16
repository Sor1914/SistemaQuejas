using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BE_QUEJAS.Services
{
    public class clsLoginService
    {
        public async Task ConsumeApi( )
        {
            var httpClient = new HttpClient();

            // Configurar la cabecera Content-Type a application/json
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Definir el objeto que se enviará en la solicitud POST
            var objeto = new
            {
                Usuario = "JSOR",
                Pass = "Sor123",                
            };

            // Serializar el objeto a JSON
            var json = JsonConvert.SerializeObject(objeto);

            // Enviar la solicitud POST a la API y obtener la respuesta
            var respuesta = await httpClient.PostAsync("http://localhost:61342/API/LOGIN/AUTENTICAR", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

            // Leer la respuesta como una cadena JSON
            var respuestaJson = await respuesta.Content.ReadAsStringAsync();

            // Deserializar la respuesta a un objeto
            var resultado = JsonConvert.DeserializeObject(respuestaJson);
        }

    }
}
