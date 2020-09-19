using Microsoft.EntityFrameworkCore.Migrations;

namespace Seeker.Migrations
{
    public partial class AddAssignUserToJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssigndUserId",
                table: "Jobs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_AssigndUserId",
                table: "Jobs",
                column: "AssigndUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_AssigndUserId",
                table: "Jobs",
                column: "AssigndUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_AssigndUserId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_AssigndUserId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "AssigndUserId",
                table: "Jobs");
        }
    }
}
