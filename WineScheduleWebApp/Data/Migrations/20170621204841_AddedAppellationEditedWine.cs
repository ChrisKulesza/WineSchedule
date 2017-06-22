using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WineScheduleWebApp.Data.Migrations
{
    public partial class AddedAppellationEditedWine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppellationId",
                table: "Wine",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Appellation",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appellation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wine_AppellationId",
                table: "Wine",
                column: "AppellationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wine_Appellation_AppellationId",
                table: "Wine",
                column: "AppellationId",
                principalTable: "Appellation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wine_Appellation_AppellationId",
                table: "Wine");

            migrationBuilder.DropTable(
                name: "Appellation");

            migrationBuilder.DropIndex(
                name: "IX_Wine_AppellationId",
                table: "Wine");

            migrationBuilder.DropColumn(
                name: "AppellationId",
                table: "Wine");
        }
    }
}
