using Pitzam.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pitzam.Services
{
    public class AuthService
    {
        private readonly IStorageProvider _storage;
        private readonly HttpClient _http;
        private const string CurrentUserKey = "pitzam_current_user";

        public event Action? OnAuthStateChanged;
        public User? CurrentUser { get; private set; }

        public AuthService(IStorageProvider storage, HttpClient http)
        {
            _storage = storage;
            _http = http;
        }

        // Kullanıcı kaydı
        public async Task<(bool Success, string Message)> RegisterAsync(User user)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", user);
            if (response.IsSuccessStatusCode)
            {
                return (true, "Kayıt başarılı! Giriş yapabilirsiniz.");
            }
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return (false, error?.Message ?? "Kayıt başarısız.");
        }

        // Kullanıcı girişi
        public async Task<(bool Success, string Message)> LoginAsync(string email, string password)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                return (false, error?.Message ?? "Giriş başarısız.");
            }

            var user = await response.Content.ReadFromJsonAsync<User>();
            if (user == null) return (false, "Giriş hatası.");

            CurrentUser = user;
            await _storage.SetItemAsync(CurrentUserKey, user);
            OnAuthStateChanged?.Invoke();

            return (true, "Giriş başarılı!");
        }

        // Kullanıcı çıkışı
        public async Task LogoutAsync()
        {
            CurrentUser = null;
            await _storage.RemoveItemAsync(CurrentUserKey);
            OnAuthStateChanged?.Invoke();
        }

        // Mevcut kullanıcıyı yükle (sayfa yenilendiğinde) - LocalStorage'dan session bakıyoruz
        public async Task LoadCurrentUserAsync()
        {
            try
            {
                CurrentUser = await _storage.GetItemAsync<User>(CurrentUserKey);
                OnAuthStateChanged?.Invoke();
            }
            catch
            {
                CurrentUser = null;
            }
        }

        // Kullanıcı giriş yapmış mı?
        public bool IsAuthenticated => CurrentUser != null;

        // Kullanıcı bilgilerini güncelle
        public async Task<bool> UpdateUserAsync(User updatedUser)
        {
            var response = await _http.PutAsJsonAsync("api/auth/update", updatedUser);
            if (!response.IsSuccessStatusCode) return false;

            // Mevcut kullanıcıyı güncelle (Session)
            if (CurrentUser?.Id == updatedUser.Id)
            {
                updatedUser.Password = ""; // Ensure simple object
                CurrentUser = updatedUser;
                await _storage.SetItemAsync(CurrentUserKey, updatedUser);
                OnAuthStateChanged?.Invoke();
            }

            return true;
        }

        // Eski şifre doğrulamasıyla şifre değiştir
        public async Task<(bool Success, string Message)> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var response = await _http.PostAsJsonAsync("api/auth/changepassword", new { UserId = userId, OldPassword = oldPassword, NewPassword = newPassword });
            if (response.IsSuccessStatusCode) return (true, "Şifre güncellendi.");
            
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return (false, error?.Message ?? "Şifre değiştirilemedi.");
        }

        private class ErrorResponse { public string Message { get; set; } = ""; }
    }
}

