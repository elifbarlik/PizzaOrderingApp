using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pitzam.Models;
using Pitzam.Server.Data;
using Pitzam.Server.Dtos;

namespace Pitzam.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly PizzaStoreContext _context;

        public OrdersController(PizzaStoreContext context)
        {
            _context = context;
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrderHistory([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required");

            return await _context.Orders
                .Include(o => o.CustomerInfo)
                .Where(o => o.CustomerInfo != null && o.CustomerInfo.Email == email)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    PizzaName = o.PizzaName,
                    Size = o.Size,
                    Extras = o.Extras,
                    RemovedIngredients = o.RemovedIngredients,
                    TotalPrice = o.TotalPrice,
                    CustomerId = o.CustomerId,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate,
                    CustomerInfo = o.CustomerInfo == null ? null : new CustomerDto
                    {
                        Id = o.CustomerInfo.Id,
                        FullName = o.CustomerInfo.FullName,
                        Email = o.CustomerInfo.Email,
                        Phone = o.CustomerInfo.Phone,
                        Address = o.CustomerInfo.Address
                    }
                })
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> PlaceOrder(Order order)
        {
            if (order.CustomerInfo != null)
            {
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email == order.CustomerInfo.Email);
                
                if (existingCustomer != null)
                {
                    existingCustomer.FullName = order.CustomerInfo.FullName;
                    existingCustomer.Address = order.CustomerInfo.Address;
                    existingCustomer.Phone = order.CustomerInfo.Phone;
                    order.CustomerInfo = existingCustomer;
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Return DTO
            var orderDto = new OrderDto
            {
                 Id = order.Id,
                 PizzaName = order.PizzaName,
                 Size = order.Size,
                 Extras = order.Extras,
                 RemovedIngredients = order.RemovedIngredients,
                 TotalPrice = order.TotalPrice,
                 CustomerId = order.CustomerId,
                 OrderNumber = order.OrderNumber,
                 OrderDate = order.OrderDate,
                 CustomerInfo = order.CustomerInfo == null ? null : new CustomerDto
                 {
                     Id = order.CustomerInfo.Id,
                     FullName = order.CustomerInfo.FullName,
                     Email = order.CustomerInfo.Email,
                     Phone = order.CustomerInfo.Phone,
                     Address = order.CustomerInfo.Address
                 }
            };

            return CreatedAtAction("GetOrder", new { id = order.Id }, orderDto);
        }

        [HttpGet("{id}")]
         public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.CustomerInfo)
                .Where(o => o.Id == id)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    PizzaName = o.PizzaName,
                    Size = o.Size,
                    Extras = o.Extras,
                    RemovedIngredients = o.RemovedIngredients,
                    TotalPrice = o.TotalPrice,
                    CustomerId = o.CustomerId,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate,
                    CustomerInfo = o.CustomerInfo == null ? null : new CustomerDto
                    {
                        Id = o.CustomerInfo.Id,
                        FullName = o.CustomerInfo.FullName,
                        Email = o.CustomerInfo.Email,
                        Phone = o.CustomerInfo.Phone,
                        Address = o.CustomerInfo.Address
                    }
                })
                .FirstOrDefaultAsync();

            if (order == null) return NotFound();
            return order;
        }
    }
}
