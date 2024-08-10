using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_managment_system_API.Migrations
{
    /// <inheritdoc />
    public partial class catagoryToCategory_and_subcatagoryTosubcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubCatagory",
                table: "BookCategories",
                newName: "SubCategory");

            migrationBuilder.RenameColumn(
                name: "Catagory",
                table: "BookCategories",
                newName: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubCategory",
                table: "BookCategories",
                newName: "SubCatagory");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "BookCategories",
                newName: "Catagory");
        }
    }
}
