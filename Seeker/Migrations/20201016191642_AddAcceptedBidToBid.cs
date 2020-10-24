using Microsoft.EntityFrameworkCore.Migrations;

namespace Seeker.Migrations
{
    public partial class AddAcceptedBidToBid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcceptedUserId",
                table: "Bid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBidAccepted",
                table: "Bid",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Bid_AcceptedUserId",
                table: "Bid",
                column: "AcceptedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bid_AspNetUsers_AcceptedUserId",
                table: "Bid",
                column: "AcceptedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bid_AspNetUsers_AcceptedUserId",
                table: "Bid");

            migrationBuilder.DropIndex(
                name: "IX_Bid_AcceptedUserId",
                table: "Bid");

            migrationBuilder.DropColumn(
                name: "AcceptedUserId",
                table: "Bid");

            migrationBuilder.DropColumn(
                name: "IsBidAccepted",
                table: "Bid");
        }
    }
}
