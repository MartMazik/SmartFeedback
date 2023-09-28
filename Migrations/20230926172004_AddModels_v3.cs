using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartFeedback.Migrations
{
    /// <inheritdoc />
    public partial class AddModels_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TextObjectId",
                table: "UserRatings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_TextObjectId",
                table: "UserRatings",
                column: "TextObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRatings_TextObjects_TextObjectId",
                table: "UserRatings",
                column: "TextObjectId",
                principalTable: "TextObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRatings_TextObjects_TextObjectId",
                table: "UserRatings");

            migrationBuilder.DropIndex(
                name: "IX_UserRatings_TextObjectId",
                table: "UserRatings");

            migrationBuilder.DropColumn(
                name: "TextObjectId",
                table: "UserRatings");
        }
    }
}
