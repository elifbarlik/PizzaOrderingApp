using Pitzam.Models;

namespace Pitzam.Services
{
    public class OrderStateService
    {
        public Order CurrentOrder { get; private set; } = new();

        public void SetOrder(Order order)
        {
            CurrentOrder = order;
        }

        public void ClearOrder()
        {
            CurrentOrder = new();
        }
    }
}
