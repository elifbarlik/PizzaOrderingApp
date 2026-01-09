using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Pitzam.Services;

namespace Pitzam.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly AuthService _authService;

        public AuthStateProvider(AuthService authService)
        {
            _authService = authService;
            _authService.OnAuthStateChanged += OnAuthStateChanged;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_authService.IsAuthenticated && _authService.CurrentUser != null)
            {
                var user = _authService.CurrentUser;
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var identity = new ClaimsIdentity(claims, "pitzam_auth");
                var principal = new ClaimsPrincipal(identity);
                return Task.FromResult(new AuthenticationState(principal));
            }

            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        private void OnAuthStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}

