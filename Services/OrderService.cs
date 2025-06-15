using BackOffice.Models;
using BackOffice.Rest;

namespace BackOffice.Services
{
    internal class OrderService(RestClient api) : IService<Order,Order,Order>
    {

        public Task Add(Order item, string imagen)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAllAsync() => api.ObtenerPedidosAsync();

        public Task Update(int id, Order item,string imagePath)
        {
            throw new NotImplementedException();
        }
    }
}
