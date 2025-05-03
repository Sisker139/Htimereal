using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Htime.Migrations
{
    /// <inheritdoc />
    public partial class updateCart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "Carts",
                newName: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Carts",
                newName: "Brand");
        }
    }
}
