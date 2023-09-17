using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MyWebApp.Models;

namespace MyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"C:\Users\USER\Desktop\DAEA\DAEA-2\MyWebApp\Controllers\items.json");
 // Ruta al archivo JSON

        // GET: api/items
        [HttpGet]
        public ActionResult<IEnumerable<Item>> Get()
        {
            var items = ReadItemsFromJsonFile();
            return Ok(items);
        }

        // POST: api/items
        [HttpPost]
        public ActionResult<Item> Post(Item item)
        {
            var items = ReadItemsFromJsonFile();
            item.Id = items.Count + 1;
            items.Add(item);
            WriteItemsToJsonFile(items);

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }
        // PUT: api/items/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Item updatedItem)
        {
            var items = ReadItemsFromJsonFile();
            var existingItem = items.FirstOrDefault(item => item.Id == id);

            if (existingItem == null)
            {
                return NotFound(); // Retorna 404 si el elemento no se encuentra
            }

            // Actualiza las propiedades del elemento existente con los valores del elemento actualizado
            existingItem.Name = updatedItem.Name;
            existingItem.Price = updatedItem.Price;
            // Agrega más propiedades según sea necesario

            WriteItemsToJsonFile(items);

            return NoContent(); // Retorna 204 si la actualización se realizó con éxito
        }

        // DELETE: api/items/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var items = ReadItemsFromJsonFile();
            var itemToRemove = items.FirstOrDefault(item => item.Id == id);

            if (itemToRemove == null)
            {
                return NotFound(); // Retorna 404 si el elemento no se encuentra
            }

            items.Remove(itemToRemove);
            WriteItemsToJsonFile(items);

            return NoContent(); // Retorna 204 si la eliminación se realizó con éxito
        }


        private List<Item> ReadItemsFromJsonFile()
        {
            try
            {
                var jsonString = System.IO.File.ReadAllText(jsonFilePath);
                return JsonSerializer.Deserialize<List<Item>>(jsonString);
            }
            catch (Exception ex)
            {
                // Maneja la excepción aquí o regístrala para su depuración
                Console.WriteLine($"Error al leer el archivo JSON: {ex.Message}");
                throw; // Lanza la excepción para que se propague
            }
        }


        private void WriteItemsToJsonFile(List<Item> items)
        {
            var jsonString = JsonSerializer.Serialize(items);
            System.IO.File.WriteAllText(jsonFilePath, jsonString);
        }
    }
}
