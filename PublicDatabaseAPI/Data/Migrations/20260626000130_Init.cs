using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicDatabaseAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LayoutCSS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleSize = table.Column<double>(type: "float", nullable: false),
                    TitleColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextSize = table.Column<double>(type: "float", nullable: false),
                    TextColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PicWidth = table.Column<int>(type: "int", nullable: false),
                    PicHeight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LayoutCSS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    views = table.Column<int>(type: "int", nullable: true),
                    links = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBlockPage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPageId = table.Column<int>(type: "int", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Column = table.Column<int>(type: "int", nullable: false),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlockPage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBlockPage_UserPage_UserPageId",
                        column: x => x.UserPageId,
                        principalTable: "UserPage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBlockPage_UserPageId",
                table: "UserBlockPage",
                column: "UserPageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LayoutCSS");

            migrationBuilder.DropTable(
                name: "UserBlockPage");

            migrationBuilder.DropTable(
                name: "UserPage");
        }
    }
}
