// Services/OrderStateService.cs
using Pitzam.Models;
using System.Text.Json;

namespace Pitzam.Services
{
    public class OrderStateService
    {
        public event Action? OnChange;

        // Sepette artık 'Pizza' nesneleri tutulacak
        public List<Pizza> CurrentPizzasInCart { get; private set; } = new();

        private readonly IStorageProvider? _storage;
        private readonly AuthService? _authService;
        private const string CartKeyPrefix = "pitzam_cart";
        private const string LastOrderKeyPrefix = "pitzam_last_order";
        private const string OrdersKeyPrefix = "pitzam_orders";
        private bool _cartLoadedFromStorage = false;

        public OrderStateService() { }

        public OrderStateService(IStorageProvider storage, AuthService authService)
        {
            _storage = storage;
            _authService = authService;
            _authService.OnAuthStateChanged += HandleAuthStateChanged;
        }

        // PizzaDetail.razor'un çağırdığı metot
        public void AddPizzaToOrder(Pizza pizza)
        {
            CurrentPizzasInCart.Add(pizza);
            PersistCart();
            NotifyStateChanged();
        }

        public void ClearCart()
        {
            CurrentPizzasInCart = new();
            PersistCart();
            NotifyStateChanged();
        }

        public void RemovePizzaAt(int index)
        {
            if (index >= 0 && index < CurrentPizzasInCart.Count)
            {
                CurrentPizzasInCart.RemoveAt(index);
                PersistCart();
                NotifyStateChanged();
            }
        }

        // Sepeti döndüren metot
        public List<Pizza> GetCart()
        {
            TryLoadCartFromStorage();
            return CurrentPizzasInCart;
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }

        private string GetCartKey()
        {
            // Kullanıcıya özel sepet anahtarı. Giriş yoksa guest anahtarı kullanılır
            var userId = _authService?.CurrentUser?.Id;
            return string.IsNullOrWhiteSpace(userId)
                ? $"{CartKeyPrefix}_guest"
                : $"{CartKeyPrefix}_{userId}";
        }

        private void HandleAuthStateChanged()
        {
            // Kullanıcı değiştiğinde doğru sepeti yükle ve UI'ı bilgilendir
            _cartLoadedFromStorage = false;
            TryLoadCartFromStorage();
        }

        private async void PersistCart()
        {
            if (_storage == null) return;
            try
            {
                await _storage.SetItemAsync(GetCartKey(), CurrentPizzasInCart);
            }
            catch { }
        }

        private async void TryLoadCartFromStorage()
        {
            if (_storage == null) return;
            if (_cartLoadedFromStorage) return;
            _cartLoadedFromStorage = true;
            try
            {
                var stored = await _storage.GetItemAsync<List<Pizza>>(GetCartKey());
                if (stored != null)
                {
                    CurrentPizzasInCart = stored;
                }
                else
                {
                    CurrentPizzasInCart = new();
                }
                // Yükleme tamamlandıktan sonra UI'ı bilgilendir
                NotifyStateChanged();
            }
            catch { }
        }

        public async Task PersistLastOrderAsync(Order order)
        {
            if (_storage == null) return;
            try
            {
                await _storage.SetItemAsync(GetLastOrderKey(), order);
            }
            catch { }
        }

        public async Task<Order?> GetLastOrderAsync()
        {
            if (_storage == null) return null;
            try
            {
                return await _storage.GetItemAsync<Order>(GetLastOrderKey());
            }
            catch { return null; }
        }

        private string GetLastOrderKey()
        {
            var userId = _authService?.CurrentUser?.Id;
            return string.IsNullOrWhiteSpace(userId)
                ? $"{LastOrderKeyPrefix}_guest"
                : $"{LastOrderKeyPrefix}_{userId}";
        }

        private string GetOrdersKey()
        {
            var userId = _authService?.CurrentUser?.Id;
            return string.IsNullOrWhiteSpace(userId)
                ? $"{OrdersKeyPrefix}_guest"
                : $"{OrdersKeyPrefix}_{userId}";
        }

        public async Task<List<Order>> GetOrderHistoryAsync()
        {
            if (_storage == null) return new List<Order>();
            try
            {
                var orders = await _storage.GetItemAsync<List<Order>>(GetOrdersKey());
                return orders ?? new List<Order>();
            }
            catch { return new List<Order>(); }
        }

        public async Task AddOrderToHistoryAsync(Order order)
        {
            if (_storage == null) return;
            try
            {
                var orders = await GetOrderHistoryAsync();
                // En yeni başa gelecek şekilde ekleyelim
                orders.Insert(0, order);
                await _storage.SetItemAsync(GetOrdersKey(), orders);
            }
            catch { }
        }
    }
}