using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicDatabaseAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class LayoutCSS : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LayoutCSS");
        }
    }
}
