using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sportics.Migrations
{
    public partial class Coach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Coaches",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Coaches");
        }
    }
}
