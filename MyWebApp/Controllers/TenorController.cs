using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyWebApp.Controllers
{
    [Route("tenor")]
    [ApiController]
    public class TenorController : ControllerBase
    {
        // Este método se ejecutará en una solicitud GET a /tenor/{otroValor}
        [HttpGet("{otroValor}")]
        public async Task<IActionResult> Get(string otroValor) // Agrega el parámetro otroValor
        {
            // Tu clave de API de Tenor
            string apiKey = "AIzaSyBATipyL64QK_zhIP5UtVoeUxI2TOW8EaA";

            
            int limit = 2;

            
            using (var httpClient = new HttpClient())
            {
                
                string apiUrl = $"https://api.tenor.com/v2/search?q={otroValor}&key={apiKey}&limit={limit}";

                try
                {
                    // Realizar la solicitud GET a la API de Tenor
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer y procesar la respuesta JSON
                        string json = await response.Content.ReadAsStringAsync();
                        // Aquí puedes manejar los datos de la respuesta JSON de Tenor.
                        return Ok(json); // Devuelve la respuesta JSON como Ok (código 200).
                    }
                    else
                    {
                        return BadRequest("Error en la solicitud a la API de Tenor");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error: {ex.Message}");
                }
            }
        }
    }
}
