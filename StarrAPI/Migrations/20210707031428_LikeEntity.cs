using Microsoft.EntityFrameworkCore.Migrations;

namespace StarrAPI.Migrations
{
    public partial class LikeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    SourceUserId = table.Column<int>(type: "int", nullable: false),
                    LikedUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => new { x.SourceUserId, x.LikedUserId });
                    table.ForeignKey(
                        name: "FK_Likes_GetAppUsers_LikedUserId",
                        column: x => x.LikedUserId,
                        principalTable: "GetAppUsers",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Likes_GetAppUsers_SourceUserId",
                        column: x => x.SourceUserId,
                        principalTable: "GetAppUsers",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_LikedUserId",
                table: "Likes",
                column: "LikedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");
        }
    }
}
