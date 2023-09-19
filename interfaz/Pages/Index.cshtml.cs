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

                var apiData = JsonSerializer.Deserialize<ApiResponseModel>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                });

                if (apiData?.results != null && apiData.results.Count > 0 && apiData.results[0]?.media_formats?.gif?.url != null)
                {
                    var gifUrl = apiData.results[0].media_formats.gif.url;
                   
                    
                    
                    ViewData["GifUrl"] = gifUrl;
                   
                }
                else
                {
                   
                    ViewData["Error"] = "No se encontró un GIF en la respuesta.";
                }

            }
            catch (HttpRequestException ex)
            {
                
                ViewData["Error"] = $"Error al consultar la API: {ex.Message}";
            }

            
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
}

public class MediaFormatsModel
{
    public GifModel gif { get; set; }
}

public class GifModel
{
    public string url { get; set; }
}
