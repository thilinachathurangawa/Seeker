using Microsoft.EntityFrameworkCore.Migrations;

namespace Seeker.Migrations
{
    public partial class serviceTypeAddToJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "Jobs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "Jobs");
        }
    }
}
