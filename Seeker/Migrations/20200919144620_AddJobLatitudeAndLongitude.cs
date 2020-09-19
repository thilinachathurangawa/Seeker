using Microsoft.EntityFrameworkCore.Migrations;

namespace Seeker.Migrations
{
    public partial class AddJobLatitudeAndLongitude : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobLatitude",
                table: "Jobs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobLongitude",
                table: "Jobs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobLatitude",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobLongitude",
                table: "Jobs");
        }
    }
}
