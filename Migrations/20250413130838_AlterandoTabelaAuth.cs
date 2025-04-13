using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WAPI_GS.Migrations
{
    /// <inheritdoc />
    public partial class AlterandoTabelaAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
                name: "accessToken",
                table: "tblauth");

            migrationBuilder.DropColumn(
                name: "userid",
                table: "tblauth");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "tokenAvailableUntil",
                table: "tblauth",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "accessToken",
                table: "tblauth",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "userid",
                table: "tblauth",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbldisciplina_TurmaId",
                table: "tbldisciplina",
                column: "TurmaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbldisciplina_tblturma_TurmaId",
                table: "tbldisciplina",
                column: "TurmaId",
                principalTable: "tblturma",
                principalColumn: "id");
        }
    }
}
