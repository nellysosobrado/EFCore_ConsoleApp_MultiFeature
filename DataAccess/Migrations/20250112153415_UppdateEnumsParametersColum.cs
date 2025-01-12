using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class UppdateEnumsParametersColum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParametersJson",
                table: "Shapes");

            migrationBuilder.AddColumn<double>(
                name: "Base",
                table: "Shapes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Height",
                table: "Shapes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Side",
                table: "Shapes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SideA",
                table: "Shapes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SideB",
                table: "Shapes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SideC",
                table: "Shapes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Width",
                table: "Shapes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Base",
                table: "Shapes");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Shapes");

            migrationBuilder.DropColumn(
                name: "Side",
                table: "Shapes");

            migrationBuilder.DropColumn(
                name: "SideA",
                table: "Shapes");

            migrationBuilder.DropColumn(
                name: "SideB",
                table: "Shapes");

            migrationBuilder.DropColumn(
                name: "SideC",
                table: "Shapes");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Shapes");

            migrationBuilder.AddColumn<string>(
                name: "ParametersJson",
                table: "Shapes",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");
        }
    }
}
