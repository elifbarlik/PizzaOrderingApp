using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pitzam.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pizzas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Ingredients = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pizzas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PizzaSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PizzaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzaSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PizzaSizes_Pizzas_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PizzaName = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<string>(type: "TEXT", nullable: false),
                    Extras = table.Column<string>(type: "TEXT", nullable: false),
                    RemovedIngredients = table.Column<string>(type: "TEXT", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: true),
                    OrderNumber = table.Column<string>(type: "TEXT", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Pizzas",
                columns: new[] { "Id", "Description", "ImageUrl", "Ingredients", "Name" },
                values: new object[,]
                {
                    { 1, "Klasik İtalyan lezzeti. Domates sosu, taze mozzarella ve fesleğen.", "images/margherita.jpg", "[\"Mozzarella\",\"Domates Sosu\",\"Fesle\\u011Fen\"]", "Margherita" },
                    { 2, "Bol pepperoni ve mozzarella peyniri ile acı sevenlere özel.", "images/pizza.jpg", "[\"Mozzarella\",\"Domates Sosu\",\"Pepperoni\"]", "Pepperoni" },
                    { 3, "Taze sebzelerle dolu. Biber, mantar, zeytin ve mozzarella.", "images/pizza.jpg", "[\"Mozzarella\",\"Domates Sosu\",\"Biber\",\"Mantar\",\"Zeytin\"]", "Vejetaryen" },
                    { 4, "Geleneksel lezzet. Bol sucuk, mozzarella ve domates sosu.", "images/pizza.jpg", "[\"Mozzarella\",\"Domates Sosu\",\"Sucuk\"]", "Sucuklu" },
                    { 5, "Özel barbekü sosu, tavuk parçaları ve soğan ile dumanlı lezzet.", "images/pizza.jpg", "[\"Mozzarella\",\"BBQ Sosu\",\"Tavuk\",\"So\\u011Fan\"]", "Tavuklu BBQ" },
                    { 6, "Tam bir peynir şöleni. Mozzarella, cheddar, parmesan ve beyaz peynir.", "images/pizza.jpg", "[\"Mozzarella\",\"Cheddar\",\"Parmesan\",\"Beyaz Peynir\"]", "Dört Peynirli" },
                    { 7, "Denizden gelen lezzet. Ton balığı, zeytin, mısır ve mozzarella.", "images/pizza.jpg", "[\"Mozzarella\",\"Ton Bal\\u0131\\u011F\\u0131\",\"Zeytin\",\"M\\u0131s\\u0131r\"]", "Ton Balıklı" },
                    { 8, "Güçlü bir Anadolu tadı. Kavurma, soğan ve mozzarella.", "images/pizza.jpg", "[\"Mozzarella\",\"Domates Sosu\",\"Kavurma\",\"So\\u011Fan\"]", "Kavurmalı" },
                    { 9, "Deniz tutkunlarına özel. Karides, kalamar ve sarımsaklı sos.", "images/pizza.jpg", "[\"Mozzarella\",\"Karides\",\"Kalamar\",\"Sar\\u0131msak\"]", "Deniz Mahsullü" },
                    { 10, "Acı ve lezzetli bir macera. Jalapeno, mısır ve kırmızı biber.", "images/pizza.jpg", "[\"Mozzarella\",\"Jalapeno\",\"M\\u0131s\\u0131r\",\"K\\u0131rm\\u0131z\\u0131 Biber\"]", "Meksika" }
                });

            migrationBuilder.InsertData(
                table: "PizzaSizes",
                columns: new[] { "Id", "PizzaId", "Price", "Size" },
                values: new object[,]
                {
                    { 1, 1, 120.00m, "Küçük" },
                    { 2, 1, 156.00m, "Orta" },
                    { 3, 1, 192.00m, "Büyük" },
                    { 4, 2, 140.00m, "Küçük" },
                    { 5, 2, 182.00m, "Orta" },
                    { 6, 2, 224.00m, "Büyük" },
                    { 7, 3, 130.00m, "Küçük" },
                    { 8, 3, 169.00m, "Orta" },
                    { 9, 3, 208.00m, "Büyük" },
                    { 10, 4, 150.00m, "Küçük" },
                    { 11, 4, 195.00m, "Orta" },
                    { 12, 4, 240.00m, "Büyük" },
                    { 13, 5, 155.00m, "Küçük" },
                    { 14, 5, 201.50m, "Orta" },
                    { 15, 5, 248.00m, "Büyük" },
                    { 16, 6, 160.00m, "Küçük" },
                    { 17, 6, 208.00m, "Orta" },
                    { 18, 6, 256.00m, "Büyük" },
                    { 19, 7, 150.00m, "Küçük" },
                    { 20, 7, 195.00m, "Orta" },
                    { 21, 7, 240.00m, "Büyük" },
                    { 22, 8, 170.00m, "Küçük" },
                    { 23, 8, 221.00m, "Orta" },
                    { 24, 8, 272.00m, "Büyük" },
                    { 25, 9, 180.00m, "Küçük" },
                    { 26, 9, 234.00m, "Orta" },
                    { 27, 9, 288.00m, "Büyük" },
                    { 28, 10, 165.00m, "Küçük" },
                    { 29, 10, 214.50m, "Orta" },
                    { 30, 10, 264.00m, "Büyük" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PizzaSizes_PizzaId",
                table: "PizzaSizes",
                column: "PizzaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PizzaSizes");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Pizzas");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
