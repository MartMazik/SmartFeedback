using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartFeedback.Migrations
{
    /// <inheritdoc />
    public partial class AddModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TextObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Text = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    TechnicalText = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    AnalogCount = table.Column<int>(type: "integer", nullable: false),
                    UserRatingCount = table.Column<int>(type: "integer", nullable: false),
                    RatingSum = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextObjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    IsLike = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRatings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConnectTextsObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    FirstTextObjectId = table.Column<int>(type: "integer", nullable: false),
                    SecondTextObjectId = table.Column<int>(type: "integer", nullable: false),
                    Coincidence = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectTextsObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectTextsObjects_TextObjects_FirstTextObjectId",
                        column: x => x.FirstTextObjectId,
                        principalTable: "TextObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectTextsObjects_TextObjects_SecondTextObjectId",
                        column: x => x.SecondTextObjectId,
                        principalTable: "TextObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectTextsObjects_FirstTextObjectId",
                table: "ConnectTextsObjects",
                column: "FirstTextObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectTextsObjects_SecondTextObjectId",
                table: "ConnectTextsObjects",
                column: "SecondTextObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TextObjects_ProjectId",
                table: "TextObjects",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectTextsObjects");

            migrationBuilder.DropTable(
                name: "UserRatings");

            migrationBuilder.DropTable(
                name: "TextObjects");
        }
    }
}
