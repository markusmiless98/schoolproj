using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicSchoolProj.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserPageBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPageBlock_UserPage_UserPageId",
                table: "UserPageBlock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPageBlock",
                table: "UserPageBlock");

            migrationBuilder.RenameTable(
                name: "UserPageBlock",
                newName: "UserBlockPage");

            migrationBuilder.RenameIndex(
                name: "IX_UserPageBlock_UserPageId",
                table: "UserBlockPage",
                newName: "IX_UserBlockPage_UserPageId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "UserBlockPage",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "UserBlockPage",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBlockPage",
                table: "UserBlockPage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBlockPage_UserPage_UserPageId",
                table: "UserBlockPage",
                column: "UserPageId",
                principalTable: "UserPage",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBlockPage_UserPage_UserPageId",
                table: "UserBlockPage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBlockPage",
                table: "UserBlockPage");

            migrationBuilder.RenameTable(
                name: "UserBlockPage",
                newName: "UserPageBlock");

            migrationBuilder.RenameIndex(
                name: "IX_UserBlockPage_UserPageId",
                table: "UserPageBlock",
                newName: "IX_UserPageBlock_UserPageId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "UserPageBlock",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "UserPageBlock",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPageBlock",
                table: "UserPageBlock",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPageBlock_UserPage_UserPageId",
                table: "UserPageBlock",
                column: "UserPageId",
                principalTable: "UserPage",
                principalColumn: "Id");
        }
    }
}
