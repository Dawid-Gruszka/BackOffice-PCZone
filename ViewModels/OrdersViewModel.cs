using BackOffice.Models;
using BackOffice.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;


namespace BackOffice.ViewModels
{
    internal partial class OrdersViewModel : ObservableObject
    {
        private readonly IService<Order,Order,Order> _orderService;

        [ObservableProperty]
        private ObservableCollection<Order> _orders;

        public OrdersViewModel(IService<Order, Order, Order> orderService)
        {
            _orderService = orderService;
            _orders = new ObservableCollection<Order>();

            LoadOrdersAsync();
        }

        private async void LoadOrdersAsync()
        {
            var result = await _orderService.GetAllAsync();
            Orders = new ObservableCollection<Order>(result);
        }
    }
}
