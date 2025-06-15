using BackOffice.Models;
using BackOffice.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Net.Http;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace BackOffice.ViewModels
{
    internal partial class ProductViewModel : ObservableObject
    {
        private readonly IService<Product,ProductCreate,ProductUpdate> _productService;
        private readonly IService<Category, Category, Category> _categoryService;

        [ObservableProperty]
        private List<Category> _categories = new();

        [ObservableProperty]
        private List<Product> _products = new();

        [ObservableProperty]
        private Product _selectedProduct = new();

        [ObservableProperty]
        private string? productName;

        [ObservableProperty]
        private string? productDescription;

        [ObservableProperty]
        private int? productCategory;

        [ObservableProperty]
        private int? productQuantity;

        [ObservableProperty]
        private int? productPrice;

        [ObservableProperty]
        private int productId;

        [ObservableProperty]
        private string? searchText;

        private string? _imagenRuta;
        public string? ImagenRuta { get => _imagenRuta; set => SetProperty(ref _imagenRuta, value); }

        public ICollectionView FilteredProducts { get; private set; }

        public ProductViewModel(IService<Product, ProductCreate, ProductUpdate> productService,
                                IService<Category, Category, Category> categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            FilteredProducts = CollectionViewSource.GetDefaultView(_products);
            FilteredProducts.Filter = FilterProducts;

            LoadProductsAsync();
        }

        [RelayCommand]
        private async Task AddProductCommand()
        {
            if (string.IsNullOrEmpty(ImagenRuta))
            {
                System.Windows.MessageBox.Show("No hay imagen seleccionada.", "Aviso", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            try
            {
                var nuevoProducto = new ProductCreate
                {
                    Name = ProductName!,
                    Description = ProductDescription!,
                    Quantity = ProductQuantity ?? 0,
                    Price = ProductPrice ?? 0,
                    CategoryId = ProductCategory ?? 0,
                };

                await _productService.Add(nuevoProducto, ImagenRuta!);

                Products = await _productService.GetAllAsync();
                FilteredProducts = CollectionViewSource.GetDefaultView(Products);
                FilteredProducts.Filter = FilterProducts;
                OnPropertyChanged(nameof(FilteredProducts));
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al crear producto: {ex.Message}");
            }
        }


        [RelayCommand]
        private async Task DeleteProductCommand()
        {
            if (SelectedProduct != null)
            {
                try
                {
                    bool result = await _productService.Delete(SelectedProduct.id);
                    if (result)
                    {
                        Products.Remove(SelectedProduct);
                        FilteredProducts.Refresh();
                        SelectedProduct = null;
                    }
                }
                catch (HttpRequestException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al eliminar producto: {ex.Message}");
                }
            }
        }

        [RelayCommand]
        private async Task UpdateProductCommand()
        {
            if (SelectedProduct != null)
            {
                if ((ProductQuantity ?? 0) < 0)
                {
                    System.Diagnostics.Debug.WriteLine("No puede menor que cero.");
                    return;
                }
                if ((ProductPrice ?? 0) < 0)
                {
                    System.Diagnostics.Debug.WriteLine("No puede menor que cero.");
                    return;
                }
                try
                {
                    var productoActualizado = new ProductUpdate
                    {
                        name = ProductName!,
                        description = ProductDescription!,
                        quantity = ProductQuantity ?? 0,
                        price = ProductPrice ?? 0,
                        category_id = ProductCategory ?? 0,
                    };
                    await _productService.Update(SelectedProduct.id, productoActualizado, ImagenRuta);
                    SelectedProduct.name = ProductName!;
                    SelectedProduct.description = ProductDescription!;
                    SelectedProduct.quantity = ProductQuantity ?? 0;
                    SelectedProduct.price = ProductPrice ?? 0;
                    SelectedProduct.category.Id = ProductCategory ?? 0;
                    FilteredProducts.Refresh();
                }
                catch (HttpRequestException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al actualizar producto: {ex.Message}");
                }
            }
        }

        [RelayCommand]
        private void SeleccionarImagenCommand()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Seleccionar imagen del producto",
                Filter = "Archivos de imagen (*.jpg;*.png)|*.jpg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagenRuta = openFileDialog.FileName;
                OnPropertyChanged(nameof(ProductoImagenSource));
            }
        }

        [RelayCommand]
        private async Task RefreshCategories()
        {
            Categories = await _categoryService.GetAllAsync();
        }

        private bool FilterProducts(object obj)
        {
            if (obj is Product product)
            {
                return string.IsNullOrWhiteSpace(SearchText) ||
                       product.name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        partial void OnSearchTextChanged(string? value)
        {
            FilteredProducts?.Refresh();
        }

        partial void OnSelectedProductChanged(Product value)
        {
            if (value != null)
            {
                ProductId = value.id;
                ProductName = value.name;
                ProductDescription = value.description;
                ProductCategory = value.category.Id;
                ProductQuantity = value.quantity;
                ProductPrice = value.price;

                ImagenRuta = null;
                OnPropertyChanged(nameof(ProductoImagenSource));
            }
        }

        private async void LoadProductsAsync()
        {
            Products = await _productService.GetAllAsync();
            Categories = await _categoryService.GetAllAsync();

            FilteredProducts = CollectionViewSource.GetDefaultView(Products);
            FilteredProducts.Filter = FilterProducts;
            OnPropertyChanged(nameof(FilteredProducts));
        }

        public ImageSource? ProductoImagenSource
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(ImagenRuta) && System.IO.File.Exists(ImagenRuta))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(ImagenRuta, UriKind.Absolute);
                        bitmap.EndInit();
                        return bitmap;
                    }

                    if (SelectedProduct != null && !string.IsNullOrEmpty(SelectedProduct.image_url))
                    {
                        string baseUrl = "http://localhost:8000";
                        string imageUrl = SelectedProduct.image_url.StartsWith("http")
                            ? SelectedProduct.image_url
                            : $"{baseUrl}{SelectedProduct.image_url}";

                        return new BitmapImage(new Uri(imageUrl));
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Error al cargar la imagen del producto.");
                }
                return null;
            }
        }
    }
}
