using Microsoft.EntityFrameworkCore.Migrations;

namespace Sportics.Migrations
{
    public partial class Offer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWeeklyOffer",
                table: "Memberships",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWeeklyOffer",
                table: "Memberships");
        }
    }
}
