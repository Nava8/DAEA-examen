using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace interfaz.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string Query { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var apiUrl = "http://localhost:5248/tenor/";
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetStringAsync(apiUrl + Query);

                // Deserializar la respuesta JSON usando System.Text.Json
                var apiData = JsonSerializer.Deserialize<ApiResponseModel>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Esto permite que coincida con las propiedades en minúsculas en el JSON
                });

                // Verificar si hay resultados y que el primer resultado tenga un GIF
                // Verificar si hay resultados y que el primer resultado tenga un GIF
                if (apiData?.results != null && apiData.results.Count > 0 && apiData.results[0]?.media_formats?.gif?.url != null)
                {
                    var gifUrl = apiData.results[0].media_formats.gif.url;
                    var gifDescription = apiData.results[0].content_description;
                    
                    // Establecer la URL del GIF y la descripción en ViewData
                    ViewData["GifUrl"] = gifUrl;
                    ViewData["Description"] = gifDescription;
                }
                else
                {
                    // Manejar el caso en el que no se encuentra un GIF
                    ViewData["Error"] = "No se encontró un GIF en la respuesta.";
                }

            }
            catch (HttpRequestException ex)
            {
                // Puedes manejar los errores aquí y mostrar un mensaje de error en la vista
                ViewData["Error"] = $"Error al consultar la API: {ex.Message}";
            }

            // Redirigir de nuevo a la misma página
            return Page();
        }


    }
}


public class ApiResponseModel
{
    public List<ResultModel> results { get; set; }
}

public class ResultModel
{
    public MediaFormatsModel media_formats { get; set; }
    public string content_description { get; set; }
}

public class ContentDescriptionModel
{
    public string content_description { get; set;}
}

public class MediaFormatsModel
{
    public GifModel gif { get; set; }
}

public class GifModel
{
    public string url { get; set; }
}
