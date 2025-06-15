using BackOffice.Models;
using BackOffice.Rest;

namespace BackOffice.Services
{
    class ProductService(RestClient api) : IService<Product,ProductCreate,ProductUpdate>
    {
        public Task Add(ProductCreate item, string imagen) => api.CrearProductoAsync(item, imagen);

        public Task<bool> Delete(int id) => api.EliminarProducto(id);

        public async Task<Product> Get(int id) => await api.ObtenerProductoById(id);

        public async Task<List<Product>> GetAllAsync() => await api.ObtenerProductos();

        public Task Update(int id, ProductUpdate product, string imagePath) => api.EditarProducto(id, product, imagePath);
    }
}
