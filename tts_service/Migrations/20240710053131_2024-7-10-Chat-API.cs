using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tts_service.Migrations
{
    /// <inheritdoc />
    public partial class _2024710ChatAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UniqueId",
                table: "Users",
                newName: "Guid");

            migrationBuilder.AddColumn<int>(
                name: "LastSessionId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TtsCalls",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ChatSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserGuid",
                table: "ChatSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserGuid",
                table: "ChatContents",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSessionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "ChatContents");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Users",
                newName: "UniqueId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TtsCalls",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
