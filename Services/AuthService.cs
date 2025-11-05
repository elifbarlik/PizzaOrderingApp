using Pitzam.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Pitzam.Services
{
    public class AuthService
    {
        private readonly IStorageProvider _storage;
        private const string UsersKey = "pitzam_users";
        private const string CurrentUserKey = "pitzam_current_user";

        public event Action? OnAuthStateChanged;
        public User? CurrentUser { get; private set; }

        public AuthService(IStorageProvider storage)
        {
            _storage = storage;
        }

        // Kullanıcıları LocalStorage'dan yükle
        private async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var usersJson = await _storage.GetItemAsync<string>(UsersKey);
                if (string.IsNullOrEmpty(usersJson))
                {
                    return new List<User>();
                }
                return JsonSerializer.Deserialize<List<User>>(usersJson) ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        // Kullanıcıları LocalStorage'a kaydet
        private async Task SaveUsersAsync(List<User> users)
        {
            try
            {
                var usersJson = JsonSerializer.Serialize(users);
                await _storage.SetItemAsync(UsersKey, usersJson);
            }
            catch { }
        }

        // Şifreyi hash'le (basit bir hash - gerçek uygulamada BCrypt veya benzeri kullanılmalı)
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // Kullanıcı kaydı
        public async Task<(bool Success, string Message)> RegisterAsync(User user)
        {
            var users = await GetUsersAsync();

            // Email kontrolü
            if (users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return (false, "Bu e-posta adresi zaten kullanılıyor.");
            }

            // Şifreyi hash'le
            user.Password = HashPassword(user.Password);
            user.CreatedAt = DateTime.Now;

            users.Add(user);
            await SaveUsersAsync(users);

            return (true, "Kayıt başarılı! Giriş yapabilirsiniz.");
        }

        // Kullanıcı girişi
        public async Task<(bool Success, string Message)> LoginAsync(string email, string password)
        {
            var users = await GetUsersAsync();
            var hashedPassword = HashPassword(password);

            var user = users.FirstOrDefault(u => 
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && 
                u.Password == hashedPassword);

            if (user == null)
            {
                return (false, "E-posta veya şifre hatalı.");
            }

            // Şifreyi geri göndermeden kullanıcıyı kaydet
            var userWithoutPassword = new User
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                CreatedAt = user.CreatedAt,
                Password = "" // Şifreyi saklamıyoruz
            };

            CurrentUser = userWithoutPassword;
            await _storage.SetItemAsync(CurrentUserKey, userWithoutPassword);
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

        // Mevcut kullanıcıyı yükle (sayfa yenilendiğinde)
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
            var users = await GetUsersAsync();
            var userIndex = users.FindIndex(u => u.Id == updatedUser.Id);

            if (userIndex == -1)
                return false;

            // Şifre değişmemişse eski şifreyi koru
            if (string.IsNullOrEmpty(updatedUser.Password))
            {
                updatedUser.Password = users[userIndex].Password;
            }
            else
            {
                updatedUser.Password = HashPassword(updatedUser.Password);
            }

            users[userIndex] = updatedUser;
            await SaveUsersAsync(users);

            // Mevcut kullanıcıyı güncelle
            if (CurrentUser?.Id == updatedUser.Id)
            {
                updatedUser.Password = "";
                CurrentUser = updatedUser;
                await _storage.SetItemAsync(CurrentUserKey, updatedUser);
                OnAuthStateChanged?.Invoke();
            }

            return true;
        }

        // Eski şifre doğrulamasıyla şifre değiştir
        public async Task<(bool Success, string Message)> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var users = await GetUsersAsync();
            var userIndex = users.FindIndex(u => u.Id == userId);
            if (userIndex == -1)
                return (false, "Kullanıcı bulunamadı.");

            var currentHashed = users[userIndex].Password;
            var oldHashed = HashPassword(oldPassword);
            if (!string.Equals(currentHashed, oldHashed, StringComparison.Ordinal))
            {
                return (false, "Mevcut şifre hatalı.");
            }

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                return (false, "Yeni şifre en az 6 karakter olmalı.");
            }

            users[userIndex].Password = HashPassword(newPassword);
            await SaveUsersAsync(users);

            // CurrentUser şifresini boş olarak güncel tut (parola gönderilmez)
            if (CurrentUser?.Id == userId)
            {
                await _storage.SetItemAsync(CurrentUserKey, CurrentUser);
                OnAuthStateChanged?.Invoke();
            }

            return (true, "Şifre güncellendi.");
        }
    }
}

