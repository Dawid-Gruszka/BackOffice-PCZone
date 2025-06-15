using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using BackOffice.Models;
using BackOffice.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels
{
    internal partial class CategoryViewModel : ObservableObject
    {
        private readonly IService<Category, Category, Category> _categoryService;

        [ObservableProperty]
        private List<Category> _categories;

        [ObservableProperty]
        private string? searchText;

        [ObservableProperty]
        private Category _selectedCategory;

        [ObservableProperty]
        private int categoryId;

        [ObservableProperty]
        private string? categoryName;

        private string? _imagenRuta;
        public string? ImagenRuta
        {
            get => _imagenRuta;
            set => SetProperty(ref _imagenRuta, value);
        }

        public ImageSource? CategoriaImagenSource
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

                    if (SelectedCategory != null && !string.IsNullOrEmpty(SelectedCategory.image_url))
                    {
                        string baseUrl = "http://localhost:8000";
                        string imageUrl = SelectedCategory.image_url.StartsWith("http")
                            ? SelectedCategory.image_url
                            : $"{baseUrl}{SelectedCategory.image_url}";

                        return new BitmapImage(new Uri(imageUrl));
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Error al cargar la imagen de la categoría.");
                }

                return null;
            }
        }

        public ICollectionView FilteredCategories { get; private set; }

        [RelayCommand]
        private async Task AddCategoryCommand()
        {
            if (!string.IsNullOrWhiteSpace(CategoryName))
            {
                if (string.IsNullOrEmpty(ImagenRuta))
                {
                    System.Windows.MessageBox.Show("No hay imagen seleccionada.", "Aviso", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
                try
                {
                    var nuevaCategoria = new Category
                    {
                        name = CategoryName
                    };

                    await _categoryService.Add(nuevaCategoria, ImagenRuta);

                    Categories.Add(nuevaCategoria);
                    FilteredCategories.Refresh();

                    CategoryName = string.Empty;
                    ImagenRuta = null;
                    OnPropertyChanged(nameof(CategoriaImagenSource));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al agregar categoría: {ex.Message}");
                }
            }
        }

        [RelayCommand]
        private async Task UpdateCategoryCommand()
        {
            if (SelectedCategory != null)
            {
                try
                {
                    var categoriaEditada = new Category
                    {
                        name = CategoryName!
                    };

                    await _categoryService.Update(SelectedCategory.Id, categoriaEditada, ImagenRuta);
                        SelectedCategory.name = CategoryName!;
                        FilteredCategories.Refresh();
                        ImagenRuta = null;
                        OnPropertyChanged(nameof(CategoriaImagenSource));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al editar categoría: {ex.Message}");
                }
            }
        }

        [RelayCommand]
        private void SeleccionarImagenCommand()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Seleccionar imagen de la categoría",
                Filter = "Archivos de imagen (*.jpg;*.png)|*.jpg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagenRuta = openFileDialog.FileName;
                OnPropertyChanged(nameof(CategoriaImagenSource));
            }
        }

        [RelayCommand]
        private async Task DeleteCategoryCommand()
        {
            if (SelectedCategory != null)
            {
                try
                {
                    await _categoryService.Delete(SelectedCategory.Id);
                    Categories.Remove(SelectedCategory);
                    FilteredCategories.Refresh();
                    SelectedCategory = null;
                    CategoryName = string.Empty;
                    ImagenRuta = null;
                    OnPropertyChanged(nameof(CategoriaImagenSource));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al eliminar categoría: {ex.Message}");
                }
            }
        }

        public CategoryViewModel(IService<Product, ProductCreate, ProductUpdate> productService,
                                IService<Category, Category, Category> categoryService)
        {
            _categoryService = categoryService;
            _categories = new List<Category>();
            _selectedCategory = new Category();

            FilteredCategories = CollectionViewSource.GetDefaultView(_categories);
            FilteredCategories.Filter = FilterCategories;

            LoadCategoriesAsync();
        }

        private bool FilterCategories(object obj)
        {
            if (obj is Category category)
            {
                return string.IsNullOrWhiteSpace(SearchText) ||
                       category.name.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        partial void OnSearchTextChanged(string? value)
        {
            FilteredCategories?.Refresh();
        }

        partial void OnSelectedCategoryChanged(Category value)
        {
            if (value != null)
            {
                CategoryId = value.Id;
                CategoryName = value.name;
                ImagenRuta = null;
                OnPropertyChanged(nameof(CategoriaImagenSource));
            }
        }

        private async void LoadCategoriesAsync()
        {
            Categories = await _categoryService.GetAllAsync();

            FilteredCategories = CollectionViewSource.GetDefaultView(Categories);
            FilteredCategories.Filter = FilterCategories;
            OnPropertyChanged(nameof(FilteredCategories));
        }
    }
}
