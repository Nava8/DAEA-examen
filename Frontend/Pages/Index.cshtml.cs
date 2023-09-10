using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http.Headers;


namespace Frontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public List<Product> Products { get; set; } = new List<Product>();

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://ip172-18-0-35-cjuh2a8gftqg00abhl5g-8080.direct.labs.play-with-docker.com/");
        }

        public async Task OnGetAsync()
        {
            // Realizar una solicitud GET a la URL de la API.
            var response = await _httpClient.GetAsync("api/items");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Products = JsonSerializer.Deserialize<List<Product>>(content);
            }
            else
            {
                // Manejar el caso en el que la solicitud no sea exitosa (por ejemplo, registro de errores).
            }
        }

        // Los métodos OnPost y GetProductsAsJson permanecen sin cambios.

        public async Task OnPost(int id, string name, double price)
        {
            // Crear un nuevo producto para agregar a la API.
            var newProduct = new Product { id = id, name = name, price = price };

            // Serializar el nuevo producto a JSON.
            var productJson = JsonSerializer.Serialize(newProduct);

            // Crear el contenido de la solicitud HTTP con el producto en formato JSON.
            var content = new StringContent(productJson, Encoding.UTF8, "application/json");

            // Realizar una solicitud HTTP POST a la API para agregar el nuevo producto.
            var response = await _httpClient.PostAsync("api/items", content);

            if (response.IsSuccessStatusCode)
            {
                // Si la solicitud es exitosa, puedes optar por actualizar la lista de productos localmente
                // para que la vista muestre el nuevo producto inmediatamente.
                Products.Add(newProduct);
            }
            else
            {
                // Manejar el caso en el que la solicitud no sea exitosa (por ejemplo, registro de errores).
                // Puedes agregar registros de depuración o manejo de errores aquí.
            }
        }

        public async Task OnPostDeleteAsync(int id)
        {
            // Realizar una solicitud HTTP DELETE a la API para eliminar el producto por su ID.
            var response = await _httpClient.DeleteAsync($"api/items/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Si la solicitud es exitosa, también eliminamos el producto localmente
                // para que la vista muestre la lista actualizada.
                var productToDelete = Products.FirstOrDefault(p => p.id == id);
                if (productToDelete != null)
                {
                    Products.Remove(productToDelete);
                }
            }
            else
            {
                // Manejar el caso en el que la solicitud no sea exitosa (por ejemplo, registro de errores).
                // Puedes agregar registros de depuración o manejo de errores aquí.
            }
        }


        public string GetProductsAsJson()
        {
            // Serializar la lista de productos a JSON y devolverla como una cadena JSON.
            return JsonSerializer.Serialize(Products);
        }
    }

    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
    }
}
