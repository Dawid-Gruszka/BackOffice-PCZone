using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels
{
    partial class MainViewModel(ProductViewModel productViewModel,
                                CategoryViewModel categoryViewModel,
                                OrdersViewModel ordersViewModel) : ObservableObject
    {

        [ObservableProperty]
        private object _currentView = productViewModel;

        public ProductViewModel ProductViewModel { get; } = productViewModel;
        public CategoryViewModel CategoryViewModel { get; } = categoryViewModel;

        public OrdersViewModel OrderViewModel { get; } = ordersViewModel;

        [RelayCommand]
        private void ActivateProductViewCommand() => CurrentView = ProductViewModel;

        [RelayCommand]
        private void ActivateCategoryViewCommand() => CurrentView = CategoryViewModel;

        [RelayCommand]
        private void ActivateOrdersViewCommand() => CurrentView = OrderViewModel;

        [RelayCommand]
        private void ExitApplicationCommand() => Application.Current.Shutdown();
    }
}
