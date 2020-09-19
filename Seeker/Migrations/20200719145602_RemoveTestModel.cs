using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Seeker.Migrations
{
    public partial class RemoveTestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CountryId = table.Column<Guid>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    LastUpdatedDateTime = table.Column<DateTime>(nullable: false),
                    one = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestModel", x => x.Id);
                });
        }
    }
}
