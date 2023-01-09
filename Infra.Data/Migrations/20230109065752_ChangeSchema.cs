using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Yalda_IconService");

            migrationBuilder.RenameTable(
                name: "Usages",
                schema: "Icons",
                newName: "Usages",
                newSchema: "Yalda_IconService");

            migrationBuilder.RenameTable(
                name: "Icons",
                schema: "Icons",
                newName: "Icons",
                newSchema: "Yalda_IconService");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Icons");

            migrationBuilder.RenameTable(
                name: "Usages",
                schema: "Yalda_IconService",
                newName: "Usages",
                newSchema: "Icons");

            migrationBuilder.RenameTable(
                name: "Icons",
                schema: "Yalda_IconService",
                newName: "Icons",
                newSchema: "Icons");
        }
    }
}
