using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyBasketballApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NbaGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeTeam = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AwayTeam = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GameTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NbaGames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Team = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsStarting = table.Column<bool>(type: "bit", nullable: false),
                    Stats_Points = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Stats_Rebounds = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Stats_Assists = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Stats_Steals = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Stats_Blocks = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Stats_FieldGoalPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Stats_ThreePointPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Injury_Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Injury_Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GameToday_HasGame = table.Column<bool>(type: "bit", nullable: true),
                    GameToday_Opponent = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GameToday_Time = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GameToday_IsHomeGame = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NbaGames");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
