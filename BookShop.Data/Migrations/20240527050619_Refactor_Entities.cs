using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_ProductId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Invoices_InvoiceEntityId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_WishListItems_Clients_ClientEntityId",
                table: "WishListItems");

            migrationBuilder.DropIndex(
                name: "IX_WishListItems_ClientEntityId",
                table: "WishListItems");

            migrationBuilder.DropIndex(
                name: "IX_Payments_InvoiceEntityId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ClientEntityId",
                table: "WishListItems");

            migrationBuilder.DropColumn(
                name: "InvoiceEntityId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Orders",
                newName: "PaymentMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                newName: "IX_Orders_PaymentMethodId");

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_InvoiceId",
                table: "Payments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductId",
                table: "OrderProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodId",
                table: "Orders",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Invoices_InvoiceId",
                table: "Payments",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Invoices_InvoiceId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_Payments_InvoiceId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "PaymentMethodId",
                table: "Orders",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_PaymentMethodId",
                table: "Orders",
                newName: "IX_Orders_ProductId");

            migrationBuilder.AddColumn<long>(
                name: "ClientEntityId",
                table: "WishListItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InvoiceEntityId",
                table: "Payments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WishListItems_ClientEntityId",
                table: "WishListItems",
                column: "ClientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_InvoiceEntityId",
                table: "Payments",
                column: "InvoiceEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_ProductId",
                table: "Orders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Invoices_InvoiceEntityId",
                table: "Payments",
                column: "InvoiceEntityId",
                principalTable: "Invoices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WishListItems_Clients_ClientEntityId",
                table: "WishListItems",
                column: "ClientEntityId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
