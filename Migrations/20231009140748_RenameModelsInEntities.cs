using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartFeedback.Migrations
{
    /// <inheritdoc />
    public partial class RenameModelsInEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "TextObjects",
                newName: "Content");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "TextObjects",
                newName: "Text");
        }
    }
}
