using BackOffice.Models;
using BackOffice.Properties;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace BackOffice.Rest
{
    class RestClient
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        public List<Product> ItemsP { get; set; }
        public List<Category> ItemsC { get; set; }

        public RestClient()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        public async Task<List<Product>> ObtenerProductos() => await _client.GetFromJsonAsync<List<Product>>(Settings.Default.Uri + "productos", _serializerOptions);
        public async Task<Product> ObtenerProductoById(int id) => await _client.GetFromJsonAsync<Product>(Settings.Default.Uri + "productos/" + id, _serializerOptions);

        public async Task<List<Category>> ObtenerCategorias() => await _client.GetFromJsonAsync<List<Category>>(Settings.Default.Uri + "categorias", _serializerOptions);

        public async Task<Product> CrearProductoAsync(ProductCreate nuevoProducto, string imagePath)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(nuevoProducto.Name), "name");
            content.Add(new StringContent(nuevoProducto.Description), "description");
            content.Add(new StringContent(nuevoProducto.Quantity.ToString()), "quantity");
            content.Add(new StringContent(nuevoProducto.Price.ToString()), "price");
            content.Add(new StringContent(nuevoProducto.CategoryId.ToString()), "category_id");

            var fileStream = File.OpenRead(imagePath);
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            content.Add(fileContent, "image", Path.GetFileName(imagePath));

            var response = await _client.PostAsync(Settings.Default.Uri + "productos", content);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Product>(_serializerOptions);
        }


        public async Task<bool> EliminarProducto(int id)
        {
            var response = await _client.DeleteAsync(Settings.Default.Uri + "productos/" + id);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception("Error al eliminar el producto: " + response.ReasonPhrase);
            }
        }

        public async Task<bool> EditarProducto(int id, ProductUpdate productoActualizado, string? imagePath = null)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(productoActualizado.name), "name");
            content.Add(new StringContent(productoActualizado.description), "description");
            content.Add(new StringContent(productoActualizado.quantity.ToString()), "quantity");
            content.Add(new StringContent(productoActualizado.price.ToString()), "price");
            content.Add(new StringContent(productoActualizado.category_id.ToString()), "category_id");

            if (!string.IsNullOrWhiteSpace(imagePath) && File.Exists(imagePath))
            {
                var fileStream = File.OpenRead(imagePath);
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                content.Add(fileContent, "image", Path.GetFileName(imagePath));
            }

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), Settings.Default.Uri + "productos/" + id)
            {
                Content = content
            };

            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al editar producto: {response.StatusCode} - {error}");
        }


        public async Task<Category> CrearCategoriaAsync(Category categoria, string? imagePath)
        {
            using var content = new MultipartFormDataContent();

            // Agregar nombre de la categoría
            content.Add(new StringContent(categoria.name), "name");

            // Agregar imagen si se proporciona
            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                var fileStream = File.OpenRead(imagePath);
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                content.Add(fileContent, "image", Path.GetFileName(imagePath));
            }

            var response = await _client.PostAsync(Settings.Default.Uri + "categorias", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Category>(_serializerOptions);
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al crear la categoría: {response.StatusCode} - {error}");
        }


        public async Task<bool> EditarCategoriaAsync(int id, Category categoria, string? imagePath)
        {
            using var content = new MultipartFormDataContent();

            // Agregar campos actualizados
            content.Add(new StringContent(categoria.name), "name");

            // Agregar imagen si se proporciona
            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                var fileStream = File.OpenRead(imagePath);
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                content.Add(fileContent, "image", Path.GetFileName(imagePath));
            }

            var response = await _client.PutAsync(Settings.Default.Uri + $"categorias/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al editar la categoría: {response.StatusCode} - {error}");
        }


        public async Task<bool> EliminarCategoria(int id)
        {
            var response = await _client.DeleteAsync(Settings.Default.Uri + "categorias/" + id);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new Exception("Error al eliminar la categoría: " + response.ReasonPhrase);
            }
        }

        public async Task<List<Order>> ObtenerPedidosAsync()
        {
            try
            {
                return await _client.GetFromJsonAsync<List<Order>>(Settings.Default.Uri + "order", _serializerOptions);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error al obtener los pedidos: " + ex.Message);
            }
        }
    }
}
