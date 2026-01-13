using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShoppingManager.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Code", "CreatedAt", "Description", "Image", "Name", "RefPrice", "Unit", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "LAPTOP001", new DateTime(2026, 1, 13, 4, 4, 32, 749, DateTimeKind.Utc).AddTicks(2692), "15.6-inch business laptop with Intel Core i7 processor", null, "Dell Latitude 5520 Laptop", 1299.99m, "PC", null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "MOUSE001", new DateTime(2026, 1, 13, 4, 4, 32, 749, DateTimeKind.Utc).AddTicks(2696), "Advanced wireless mouse for productivity", null, "Logitech MX Master 3 Mouse", 99.99m, "PC", null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "PAPER001", new DateTime(2026, 1, 13, 4, 4, 32, 749, DateTimeKind.Utc).AddTicks(2700), "500 sheets of white A4 copy paper", null, "A4 Copy Paper", 4.99m, "REAM", null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "PEN001", new DateTime(2026, 1, 13, 4, 4, 32, 749, DateTimeKind.Utc).AddTicks(2704), "Set of 10 blue ballpoint pens", null, "Ballpoint Pen Set", 12.50m, "SET", null },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "MONITOR001", new DateTime(2026, 1, 13, 4, 4, 32, 749, DateTimeKind.Utc).AddTicks(2706), "Full HD 1920x1080 LED monitor", null, "Samsung 24-inch Monitor", 249.99m, "PC", null }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 13, 4, 4, 32, 749, DateTimeKind.Utc).AddTicks(2089), "$2a$11$r28qkKUocikODyNzXN/4he26o2cECIISdlEudhS5Vq2vQa/jstpgu" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 13, 4, 2, 41, 586, DateTimeKind.Utc).AddTicks(2559), "$2a$11$wlyCL5p7wgtBNapxHcUMbe9Z4cV8cgLpvcTFp.4EuzcytNrKTQQOC" });
        }
    }
}
