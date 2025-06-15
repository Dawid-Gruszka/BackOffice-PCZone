using BackOffice.Models;
using BackOffice.Rest;

namespace BackOffice.Services
{
    class CategoryService(RestClient api) : IService<Category, Category, Category>
    {

        public Task Add(Category item, string imagen) => api.CrearCategoriaAsync(item, imagen);

        public async Task<bool> Delete(int id) => await api.EliminarCategoria(id);

        public Task<Category> Get(int id) => throw new NotImplementedException();

        public async Task<List<Category>> GetAllAsync() => await api.ObtenerCategorias();

        public async Task Update(int id, Category item, string imagePath) => await api.EditarCategoriaAsync(id, item, imagePath);
    }
}
