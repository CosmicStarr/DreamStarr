using Microsoft.EntityFrameworkCore.Migrations;

namespace StarrAPI.Migrations
{
    public partial class groupstodb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GetGroups",
                columns: table => new
                {
                    GroupName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetGroups", x => x.GroupName);
                });

            migrationBuilder.CreateTable(
                name: "GetConnections",
                columns: table => new
                {
                    ConnectionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetConnections", x => x.ConnectionId);
                    table.ForeignKey(
                        name: "FK_GetConnections_GetGroups_GroupName",
                        column: x => x.GroupName,
                        principalTable: "GetGroups",
                        principalColumn: "GroupName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GetConnections_GroupName",
                table: "GetConnections",
                column: "GroupName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GetConnections");

            migrationBuilder.DropTable(
                name: "GetGroups");
        }
    }
}
