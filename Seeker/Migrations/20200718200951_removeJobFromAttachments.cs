using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Seeker.Migrations
{
    public partial class removeJobFromAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Jobs_JobId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_JobId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Attachments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JobId",
                table: "Attachments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_JobId",
                table: "Attachments",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Jobs_JobId",
                table: "Attachments",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
