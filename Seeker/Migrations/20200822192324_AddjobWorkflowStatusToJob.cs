using Microsoft.EntityFrameworkCore.Migrations;

namespace Seeker.Migrations
{
    public partial class AddjobWorkflowStatusToJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "workflowStatus",
                table: "Jobs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "workflowStatus",
                table: "Jobs");
        }
    }
}
