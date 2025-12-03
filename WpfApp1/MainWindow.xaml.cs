using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using Pos.Desktop.Models;

namespace Pos.Desktop
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient = new();
        private ObservableCollection<Product> _products = new();
        private ObservableCollection<CartItem> _cartItems = new();

        private decimal _subtotal;
        private decimal _tax;
        private decimal _total;

        public MainWindow()
        {
            InitializeComponent();

            CartGrid.ItemsSource = _cartItems;

            // Use the HTTP URL from your API console output
            _httpClient.BaseAddress = new Uri("http://localhost:5171/");

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadProductsAsync();
            UpdateTotals();
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                var products = await _httpClient.GetFromJsonAsync<Product[]>("api/products");
                if (products != null)
                {
                    _products = new ObservableCollection<Product>(products);
                    ProductsList.ItemsSource = _products;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load products: {ex.Message}");
            }
        }

        private void ProductsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ProductsList.SelectedItem is Product product)
            {
                AddProductToCart(product);
            }
        }

        private void AddProductToCart(Product product)
        {
            var existing = _cartItems.FirstOrDefault(c => c.Product.Id == product.Id);
            if (existing != null)
            {
                existing.Quantity += 1;
            }
            else
            {
                _cartItems.Add(new CartItem(product));
            }

            CartGrid.Items.Refresh();
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            _subtotal = _cartItems.Sum(c => c.LineTotal);
            _tax = Math.Round(_subtotal * 0.06m, 2); // must match backend
            _total = _subtotal + _tax;

            SubtotalText.Text = $"Subtotal: {_subtotal:C}";
            TaxText.Text = $"Tax (6%): {_tax:C}";
            TotalText.Text = $"Total: {_total:C}";
        }

        private async void CompleteSale_Click(object sender, RoutedEventArgs e)
        {
            if (!_cartItems.Any())
            {
                MessageBox.Show("Cart is empty.");
                return;
            }

            try
            {
                var payload = new
                {
                    LocationCode = "MAIN",
                    Items = _cartItems.Select(c => new { ProductId = c.Product.Id, Quantity = c.Quantity }).ToList()
                };

                var response = await _httpClient.PostAsJsonAsync("api/orders", payload);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<OrderResponse>();

                MessageBox.Show($"Sale complete! Order #{result?.Id} Total: {result?.Total:C}");

                _cartItems.Clear();
                CartGrid.Items.Refresh();
                UpdateTotals();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to complete sale: {ex.Message}");
            }
        }

        private class OrderResponse
        {
            public int Id { get; set; }
            public decimal Total { get; set; }
            public string Status { get; set; } = "";
            public DateTime CreatedAtUtc { get; set; }
        }
    }
}
