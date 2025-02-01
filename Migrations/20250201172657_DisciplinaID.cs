using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WAPI_GS.Migrations
{
    /// <inheritdoc />
    public partial class DisciplinaID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "disciplinaid",
                table: "tbluserssaladias",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "disciplinaid",
                table: "tbluserssaladias");
        }
    }
}
