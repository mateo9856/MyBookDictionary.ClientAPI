using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBookDictionary.Infra.Migrations.Main
{
    public partial class TagContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTag",
                table: "ContentClasses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTag",
                table: "ContentClasses");
        }
    }
}
