using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cara.DataAccess.Migrations
{
    public partial class adEx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Blogs");
        }
    }
}
