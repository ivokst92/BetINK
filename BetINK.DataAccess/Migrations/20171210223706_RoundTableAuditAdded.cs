namespace BetINK.Web.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    using System;

    public partial class RoundTableAuditAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Rounds",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Rounds",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Rounds",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Rounds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Rounds");
        }
    }
}
