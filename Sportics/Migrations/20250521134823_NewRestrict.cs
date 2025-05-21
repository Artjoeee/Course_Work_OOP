using Microsoft.EntityFrameworkCore.Migrations;

namespace Sportics.Migrations
{
    public partial class NewRestrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientSessionRecords_Schedules_ScheduleId",
                table: "ClientSessionRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientSessionRecords_Schedules_ScheduleId",
                table: "ClientSessionRecords",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientSessionRecords_Schedules_ScheduleId",
                table: "ClientSessionRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientSessionRecords_Schedules_ScheduleId",
                table: "ClientSessionRecords",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
