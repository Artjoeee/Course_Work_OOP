using Microsoft.EntityFrameworkCore.Migrations;

namespace Sportics.Migrations
{
    public partial class Admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminReply",
                table: "SessionReviews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminReply",
                table: "CoachReviews",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminReply",
                table: "SessionReviews");

            migrationBuilder.DropColumn(
                name: "AdminReply",
                table: "CoachReviews");
        }
    }
}
