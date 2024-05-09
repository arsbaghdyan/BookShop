using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_WishLists_WishListEntityId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Invoices_InvoiceEntityId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Carts_CartEntityId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_WishLists_WishListEntityId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_WishListItems_ProductId",
                table: "WishListItems");

            migrationBuilder.DropIndex(
                name: "IX_Products_CartEntityId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Orders_InvoiceEntityId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_OrderId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_PaymentId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Clients_WishListEntityId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CartEntityId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InvoiceEntityId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WishListEntityId",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "WishListEntityId",
                table: "Products",
                newName: "CartItemEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_WishListEntityId",
                table: "Products",
                newName: "IX_Products_CartItemEntityId");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "WishLists",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_WishListItems_ProductId",
                table: "WishListItems",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_OrderId",
                table: "Invoices",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PaymentId",
                table: "Invoices",
                column: "PaymentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CartItems_CartItemEntityId",
                table: "Products",
                column: "CartItemEntityId",
                principalTable: "CartItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WishLists_Clients_Id",
                table: "WishLists",
                column: "Id",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CartItems_CartItemEntityId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_WishLists_Clients_Id",
                table: "WishLists");

            migrationBuilder.DropIndex(
                name: "IX_WishListItems_ProductId",
                table: "WishListItems");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_OrderId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_PaymentId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "CartItemEntityId",
                table: "Products",
                newName: "WishListEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CartItemEntityId",
                table: "Products",
                newName: "IX_Products_WishListEntityId");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "WishLists",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "CartEntityId",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InvoiceEntityId",
                table: "Orders",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WishListEntityId",
                table: "Clients",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WishListItems_ProductId",
                table: "WishListItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CartEntityId",
                table: "Products",
                column: "CartEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_InvoiceEntityId",
                table: "Orders",
                column: "InvoiceEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_OrderId",
                table: "Invoices",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PaymentId",
                table: "Invoices",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_WishListEntityId",
                table: "Clients",
                column: "WishListEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_WishLists_WishListEntityId",
                table: "Clients",
                column: "WishListEntityId",
                principalTable: "WishLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Invoices_InvoiceEntityId",
                table: "Orders",
                column: "InvoiceEntityId",
                principalTable: "Invoices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Carts_CartEntityId",
                table: "Products",
                column: "CartEntityId",
                principalTable: "Carts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_WishLists_WishListEntityId",
                table: "Products",
                column: "WishListEntityId",
                principalTable: "WishLists",
                principalColumn: "Id");
        }
    }
}
