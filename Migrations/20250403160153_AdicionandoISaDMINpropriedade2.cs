using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WAPI_GS.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoISaDMINpropriedade2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "isAdmin",
                table: "tbluser",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAdmin",
                table: "tbluser");
        }
    }
}
