using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WAPI_GS.Migrations
{
    /// <inheritdoc />
    public partial class NomeDaMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblauth",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    isAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    accessToken = table.Column<string>(type: "text", nullable: false),
                    TokenAvailableUntil = table.Column<long>(type: "bigint", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    requestToken = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblauth", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblsala",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    creationdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblsala", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbluser",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    creationdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastlogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    mobilephone = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    username = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbluser", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbluserssaladias",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    salaid = table.Column<int>(type: "integer", nullable: false),
                    dia = table.Column<DateOnly>(type: "date", nullable: false),
                    horainit = table.Column<int>(type: "integer", nullable: false),
                    horafinal = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbluserssaladias", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblauth");

            migrationBuilder.DropTable(
                name: "tblsala");

            migrationBuilder.DropTable(
                name: "tbluser");

            migrationBuilder.DropTable(
                name: "tbluserssaladias");
        }
    }
}
