using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBookDictionary.Infra.Migrations.Identity
{
    public partial class UpdateIdentity1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                schema: "identity",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MFACode",
                schema: "identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MFACode",
                schema: "identity",
                table: "Users");
        }
    }
}
