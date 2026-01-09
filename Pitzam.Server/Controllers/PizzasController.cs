using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pitzam.Server.Data;
using Pitzam.Server.Dtos;

namespace Pitzam.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        private readonly PizzaStoreContext _context;

        public PizzasController(PizzaStoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PizzaDto>>> GetPizzas()
        {
            return await _context.Pizzas
                .Include(p => p.Sizes)
                .Select(p => new PizzaDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Ingredients = p.Ingredients,
                    Sizes = p.Sizes.Select(s => new PizzaSizeDto
                    {
                        Id = s.Id,
                        PizzaId = s.PizzaId,
                        Size = s.Size,
                        Price = s.Price
                    }).ToList()
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PizzaDto>> GetPizza(int id)
        {
            var pizza = await _context.Pizzas
                .Include(p => p.Sizes)
                .Where(p => p.Id == id)
                .Select(p => new PizzaDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Ingredients = p.Ingredients,
                    Sizes = p.Sizes.Select(s => new PizzaSizeDto
                    {
                        Id = s.Id,
                        PizzaId = s.PizzaId,
                        Size = s.Size,
                        Price = s.Price
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (pizza == null)
            {
                return NotFound();
            }

            return pizza;
        }
    }
}
