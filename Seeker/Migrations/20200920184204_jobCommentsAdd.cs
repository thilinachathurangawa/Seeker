using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Seeker.Migrations
{
    public partial class jobCommentsAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    JobId = table.Column<Guid>(nullable: false),
                    CommentedUserId = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobComment_AspNetUsers_CommentedUserId",
                        column: x => x.CommentedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobComment_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobComment_CommentedUserId",
                table: "JobComment",
                column: "CommentedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobComment_JobId",
                table: "JobComment",
                column: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobComment");
        }
    }
}
