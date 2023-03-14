using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetaWebHook.Migrations
{
    /// <inheritdoc />
    public partial class verification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Verification",
                columns: table => new
                {
                    resource = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    eventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    flowId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Verification", x => x.resource);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Verification");
        }
    }
}
