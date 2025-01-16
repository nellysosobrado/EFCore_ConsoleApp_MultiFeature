using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstNumber = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    SecondNumber = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    Result = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    Operator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalculationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerMove = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComputerMove = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Winner = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AverageWinRate = table.Column<double>(type: "float", nullable: false),
                    GameDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shapes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShapeType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Area = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    Perimeter = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    CalculationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Width = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: true),
                    Height = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: true),
                    Side = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: true),
                    Base = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: true),
                    SideA = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: true),
                    SideB = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: true),
                    SideC = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shapes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calculations");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Shapes");
        }
    }
}
