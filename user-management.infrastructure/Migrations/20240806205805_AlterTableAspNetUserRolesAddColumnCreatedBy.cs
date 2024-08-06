using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user_management.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableAspNetUserRolesAddColumnCreatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AspNetRoles");
        }
    }
}
