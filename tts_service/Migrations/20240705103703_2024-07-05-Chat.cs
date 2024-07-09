using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tts_service.Migrations
{
    /// <inheritdoc />
    public partial class _20240705Chat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EngineId",
                table: "ChatSessions",
                newName: "TtsVoiceId");

            migrationBuilder.RenameColumn(
                name: "ChatCount",
                table: "ChatSessions",
                newName: "TtsEngineId");

            migrationBuilder.AddColumn<string>(
                name: "UniqueId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChatEngineId",
                table: "ChatSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContentCount",
                table: "ChatSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "ChatSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sender = table.Column<int>(type: "int", nullable: false),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoiceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    ChatSessionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatContents_ChatSessions_ChatSessionId",
                        column: x => x.ChatSessionId,
                        principalTable: "ChatSessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChatEngines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenRemain = table.Column<int>(type: "int", nullable: false),
                    CallCount = table.Column<int>(type: "int", nullable: false),
                    Avaliable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatEngines", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatContents_ChatSessionId",
                table: "ChatContents",
                column: "ChatSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatContents");

            migrationBuilder.DropTable(
                name: "ChatEngines");

            migrationBuilder.DropColumn(
                name: "Account",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ChatEngineId",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "ContentCount",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "ChatSessions");

            migrationBuilder.RenameColumn(
                name: "TtsVoiceId",
                table: "ChatSessions",
                newName: "EngineId");

            migrationBuilder.RenameColumn(
                name: "TtsEngineId",
                table: "ChatSessions",
                newName: "ChatCount");
        }
    }
}
