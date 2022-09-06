using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBookDictionary.Infra.Migrations.Identity
{
    public partial class UpdateIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "identity",
                table: "UsersRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "identity",
                table: "UsersRoles");
        }
    }
}
