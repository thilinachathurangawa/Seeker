using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Seeker.Migrations
{
    public partial class AddTestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CountryId = table.Column<Guid>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    LastUpdatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    one = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestModel");
        }
    }
}
