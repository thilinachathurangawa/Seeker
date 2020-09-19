using Microsoft.EntityFrameworkCore.Migrations;

namespace Seeker.Migrations
{
    public partial class AddFileUrl_To_Attachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Attachments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Attachments");
        }
    }
}
