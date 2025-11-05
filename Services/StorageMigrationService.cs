using Microsoft.JSInterop;

namespace Pitzam.Services
{
    public class StorageMigrationService
    {
        private readonly IJSRuntime _js;
        private static readonly string[] Prefixes = new[]
        {
            "pitzam_users",
            "pitzam_current_user",
            "pitzam_cart",
            "pitzam_last_order",
            "pitzam_orders"
        };

        public StorageMigrationService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task MigrateAsync()
        {
            await _js.InvokeVoidAsync("pitzamIdb.migrateFromLocalStorage", (object)Prefixes);
        }
    }
}


