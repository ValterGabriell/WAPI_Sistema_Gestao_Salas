using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WAPI_GS.Migrations
{
    /// <inheritdoc />
    public partial class AtualizandoEstrutura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurmaId",
                table: "tbldisciplina");

            migrationBuilder.AddColumn<string>(
                name: "TurmaId",
                table: "tblptd",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "tokenAvailableUntil",
                table: "tblauth",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurmaId",
                table: "tblptd");

            migrationBuilder.AddColumn<string>(
                name: "TurmaId",
                table: "tbldisciplina",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "tokenAvailableUntil",
                table: "tblauth",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
