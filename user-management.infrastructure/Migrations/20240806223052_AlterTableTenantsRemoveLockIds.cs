using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user_management.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableTenantsRemoveLockIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockIds",
                table: "Tenants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LockIds",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
