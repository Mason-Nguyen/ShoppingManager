using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingManager.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 13, 4, 27, 23, 517, DateTimeKind.Utc).AddTicks(8069));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 13, 4, 27, 23, 517, DateTimeKind.Utc).AddTicks(8074));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 13, 4, 27, 23, 517, DateTimeKind.Utc).AddTicks(8088));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 13, 4, 27, 23, 517, DateTimeKind.Utc).AddTicks(8092));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 13, 4, 27, 23, 517, DateTimeKind.Utc).AddTicks(8094));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 13, 4, 27, 23, 389, DateTimeKind.Utc).AddTicks(9388), "$2a$11$BAwmy1TmtnSjhnP5JA5vru8pjSxinjOdUWcvdn2dH59uiY4sUA1AC" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsActive", "LastLoginAt", "LastLoginIP", "LastName", "PasswordHash", "ResetToken", "ResetTokenExpiry", "Role" },
                values: new object[] { 3, new DateTime(2026, 1, 13, 4, 27, 23, 517, DateTimeKind.Utc).AddTicks(7380), "purchase@shoppingmanager.com", "Purchase", true, null, null, "Manager", "$2a$11$A1aBucbOLFgh5MNsPhjnx.gPVNwhDLOhoLWv8blLqTQxVPVhKLtN2", null, null, 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 13, 4, 20, 33, 98, DateTimeKind.Utc).AddTicks(1383));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 13, 4, 20, 33, 98, DateTimeKind.Utc).AddTicks(1387));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 13, 4, 20, 33, 98, DateTimeKind.Utc).AddTicks(1391));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 13, 4, 20, 33, 98, DateTimeKind.Utc).AddTicks(1395));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 13, 4, 20, 33, 98, DateTimeKind.Utc).AddTicks(1398));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 13, 4, 20, 32, 970, DateTimeKind.Utc).AddTicks(5609), "$2a$11$X9Ozmok7Y0YOvTr3hYwGguQPLjnb8O9EOVdaDdsyFoR9s8cJCpsxC" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsActive", "LastLoginAt", "LastLoginIP", "LastName", "PasswordHash", "ResetToken", "ResetTokenExpiry", "Role" },
                values: new object[] { 2, new DateTime(2026, 1, 13, 4, 20, 33, 98, DateTimeKind.Utc).AddTicks(597), "purchase@shoppingmanager.com", "Purchase", true, null, null, "Manager", "$2a$11$Fz.g/LEHr7NaX/krf7c06.7hxNQ.gzU2DhRcsbiSo.Q5KuqowXbh2", null, null, 5 });
        }
    }
}
