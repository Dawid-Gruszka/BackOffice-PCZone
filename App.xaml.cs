using System.Windows;
using BackOffice.Models;
using BackOffice.Rest;
using BackOffice.Services;
using BackOffice.ViewModels;
using BackOffice.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ServiceCollection services = new();

            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<ProductView>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<CategoryView>();
            services.AddTransient<CategoryViewModel>();
            services.AddTransient<OrdersView>();
            services.AddTransient<OrdersViewModel>();
            services.AddScoped<RestClient>();
            services.AddScoped<IService<Product,ProductCreate,ProductUpdate>, ProductService>();
            services.AddScoped<IService<Category,Category,Category>, CategoryService>();
            services.AddScoped<IService<Order,Order,Order>, OrderService>();

            var serviceProvider = services.BuildServiceProvider();

            var view = serviceProvider.GetService<MainWindow>();
            view.DataContext = serviceProvider.GetService<MainViewModel>();
            view.Show();
        }
    }
}
