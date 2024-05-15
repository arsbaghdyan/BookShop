using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class Configuration_ItemsEntity_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CartItems_CartItemEntityId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_WishListItems_ProductId",
                table: "WishListItems");

            migrationBuilder.DropIndex(
                name: "IX_Products_CartItemEntityId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CartItemEntityId",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_WishListItems_ProductId",
                table: "WishListItems",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WishListItems_ProductId",
                table: "WishListItems");

            migrationBuilder.AddColumn<long>(
                name: "CartItemEntityId",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WishListItems_ProductId",
                table: "WishListItems",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CartItemEntityId",
                table: "Products",
                column: "CartItemEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CartItems_CartItemEntityId",
                table: "Products",
                column: "CartItemEntityId",
                principalTable: "CartItems",
                principalColumn: "Id");
        }
    }
}
