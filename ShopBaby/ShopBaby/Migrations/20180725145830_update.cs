using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopBaby.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryID",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ProductCategoryID",
                table: "ProductCategories",
                column: "ProductCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_ProductCategories_ProductCategoryID",
                table: "ProductCategories",
                column: "ProductCategoryID",
                principalTable: "ProductCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_ProductCategories_ProductCategoryID",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_ProductCategoryID",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "ProductCategoryID",
                table: "ProductCategories");
        }
    }
}
