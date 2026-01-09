using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pitzam.Models;
using System.Text.Json;

namespace Pitzam.Server.Data
{
    public class PizzaStoreContext : DbContext
    {
        public PizzaStoreContext(DbContextOptions<PizzaStoreContext> options) : base(options)
        {
        }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PizzaSize> PizzaSizes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Value Converter for List<string> -> string (JSON)
            var stringListConverter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>());

            modelBuilder.Entity<Pizza>()
                .Property(p => p.Ingredients)
                .HasConversion(stringListConverter);

            modelBuilder.Entity<Order>()
                .Property(o => o.Extras)
                .HasConversion(stringListConverter);

            modelBuilder.Entity<Order>()
                .Property(o => o.RemovedIngredients)
                .HasConversion(stringListConverter);

            // Seed Data
            modelBuilder.Entity<Pizza>().HasData(
               new Pizza { Id = 1, Name = "Margherita", Description = "Klasik İtalyan lezzeti. Domates sosu, taze mozzarella ve fesleğen.", ImageUrl = "images/margherita.jpg", Ingredients = new List<string> { "Mozzarella", "Domates Sosu", "Fesleğen" } },
               new Pizza { Id = 2, Name = "Pepperoni", Description = "Bol pepperoni ve mozzarella peyniri ile acı sevenlere özel.", ImageUrl = "images/pizza.jpg", Ingredients = new List<string> { "Mozzarella", "Domates Sosu", "Pepperoni" } },
               new Pizza { Id = 3, Name = "Vejetaryen", Description = "Taze sebzelerle dolu. Biber, mantar, zeytin ve mozzarella.", ImageUrl = "images/pizza.jpg", Ingredients = new List<string> { "Mozzarella", "Domates Sosu", "Biber", "Mantar", "Zeytin" } },
               new Pizza { Id = 4, Name = "Sucuklu", Description = "Geleneksel lezzet. Bol sucuk, mozzarella ve domates sosu.", ImageUrl = "images/pizza.jpg", Ingredients = new List<string> { "Mozzarella", "Domates Sosu", "Sucuk" } },
               new Pizza { Id = 5, Name = "Tavuklu BBQ", Description = "Özel barbekü sosu, tavuk parçaları ve soğan ile dumanlı lezzet.", ImageUrl = "images/pizza.jpg", Ingredients = new List<string> { "Mozzarella", "BBQ Sosu", "Tavuk", "Soğan" } },
               new Pizza { Id = 6, Name = "Dört Peynirli", Description = "Tam bir peynir şöleni. Mozzarella, cheddar, parmesan ve beyaz peynir.", ImageUrl = "images/pizza.jpg", Ingredients = new List<string> { "Mozzarella", "Cheddar", "Parmesan", "Beyaz Peynir" } },
               new Pizza { Id = 7, Name = "Ton Balıklı", Description = "Denizden gelen lezzet. Ton balığı, zeytin, mısır ve mozzarella.", ImageUrl = "images/pizza.jpg", Ingredients = new List<string> { "Mozzarella", "Ton Balığı", "Zeytin", "Mısır" } },
               new Pizza { Id = 8, Name = "Kavurmalı", Description = "Güçlü bir Anadolu tadı. Kavurma, soğan ve mozzarella.", ImageUrl = "images/pizza.jpg", Ingredients = new List<string> { "Mozzarella", "Domates Sosu", "Kavurma", "Soğan" } },
               new Pizza { Id = 9, Name = "Deniz Mahsullü", Description = "Deniz tutkunlarına özel. Karides, kalamar ve sarımsaklı sos.", ImageUrl = "images/pizza.jpg", Ingredients = new List<string> { "Mozzarella", "Karides", "Kalamar", "Sarımsak" } },
               new Pizza { Id = 10, Name = "Meksika", Description = "Acı ve lezzetli bir macera. Jalapeno, mısır ve kırmızı biber.", ImageUrl = "images/pizza.jpg", Ingredients = new List<string> { "Mozzarella", "Jalapeno", "Mısır", "Kırmızı Biber" } }
            );

            // Seed Pizza Sizes
            // Simplified seeding for brevity, but adhering to the structure
            var sizes = new List<PizzaSize>();
            int sizeId = 1;

            // Helper to add sizes for a pizza
            void AddSizes(int pizzaId, decimal sm, decimal md, decimal lg)
            {
                sizes.Add(new PizzaSize { Id = sizeId++, PizzaId = pizzaId, Size = "Küçük", Price = sm });
                sizes.Add(new PizzaSize { Id = sizeId++, PizzaId = pizzaId, Size = "Orta", Price = md });
                sizes.Add(new PizzaSize { Id = sizeId++, PizzaId = pizzaId, Size = "Büyük", Price = lg });
            }

            AddSizes(1, 120.00m, 156.00m, 192.00m); // Margherita
            AddSizes(2, 140.00m, 182.00m, 224.00m); // Pepperoni
            AddSizes(3, 130.00m, 169.00m, 208.00m); // Vejetaryen
            AddSizes(4, 150.00m, 195.00m, 240.00m); // Sucuklu
            AddSizes(5, 155.00m, 201.50m, 248.00m); // Tavuklu BBQ
            AddSizes(6, 160.00m, 208.00m, 256.00m); // Dort Peynirli
            AddSizes(7, 150.00m, 195.00m, 240.00m); // Ton Balikli
            AddSizes(8, 170.00m, 221.00m, 272.00m); // Kavurmali
            AddSizes(9, 180.00m, 234.00m, 288.00m); // Deniz Mahsullu
            AddSizes(10, 165.00m, 214.50m, 264.00m); // Meksika

            modelBuilder.Entity<PizzaSize>().HasData(sizes);
        }
    }
}
