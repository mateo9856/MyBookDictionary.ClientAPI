using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBookDictionary.Infra.Migrations.Identity
{
    public partial class UpdateIdentityGUID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Guid",
                schema: "identity",
                table: "UsersRoles",
                newName: "RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersRoles",
                schema: "identity",
                table: "UsersRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersRoles",
                schema: "identity",
                table: "UsersRoles");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                schema: "identity",
                table: "UsersRoles",
                newName: "Guid");
        }
    }
}
