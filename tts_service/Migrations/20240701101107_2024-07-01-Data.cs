using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tts_service.Migrations
{
    /// <inheritdoc />
    public partial class _20240701Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastCall",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ChatSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EngineId = table.Column<int>(type: "int", nullable: false),
                    ChatCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TtsCalls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngineId = table.Column<int>(type: "int", nullable: false),
                    EngineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoiceId = table.Column<int>(type: "int", nullable: false),
                    VoiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TtsCalls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TtsEngines",
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
                    table.PrimaryKey("PK_TtsEngines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TtsVoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Engine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TtsEngineId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TtsVoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TtsVoices_TtsEngines_TtsEngineId",
                        column: x => x.TtsEngineId,
                        principalTable: "TtsEngines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TtsVoices_TtsEngineId",
                table: "TtsVoices",
                column: "TtsEngineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatSessions");

            migrationBuilder.DropTable(
                name: "TtsCalls");

            migrationBuilder.DropTable(
                name: "TtsVoices");

            migrationBuilder.DropTable(
                name: "TtsEngines");

            migrationBuilder.DropColumn(
                name: "LastCall",
                table: "Users");
        }
    }
}
