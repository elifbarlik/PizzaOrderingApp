using Pitzam.Models;

namespace Pitzam.Services
{
    public class OrderStateService
    {
        // Diğer bileşenlerin "dinleyebilmesi" için bir event
        public event Action? OnChange;

        // ARTIK BİR LİSTE TUTUYORUZ (SEPETİMİZ BU)
        public List<Order> CurrentPizzasInCart { get; private set; } = new();

        // İsmi "AddPizzaToCart" olarak değiştirdik ve listeye ekliyoruz
        public void AddPizzaToCart(Order pizzaOrder)
        {
            CurrentPizzasInCart.Add(pizzaOrder);
            
            // Sepet değişti! Event'i tetikle.
            NotifyStateChanged();
        }

        // İsmi "ClearCart" olarak değiştirdik ve listeyi temizliyoruz
        public void ClearCart()
        {
            CurrentPizzasInCart = new();
            
            // Sepet temizlendi! Event'i tetikle.
            NotifyStateChanged();
        }

        // Bu metot artık sepetin tamamını (pizzaların listesini) döndürür
        public List<Order> GetCart()
        {
            return CurrentPizzasInCart;
        }

        // OnChange event'ini çağıran yardımcı metot
        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}