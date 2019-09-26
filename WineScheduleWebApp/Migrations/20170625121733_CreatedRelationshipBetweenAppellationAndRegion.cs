using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WineScheduleWebApp.Migrations
{
    public partial class CreatedRelationshipBetweenAppellationAndRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Region",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionId",
                table: "Appellation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appellation_RegionId",
                table: "Appellation",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appellation_Region_RegionId",
                table: "Appellation",
                column: "RegionId",
                principalTable: "Region",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appellation_Region_RegionId",
                table: "Appellation");

            migrationBuilder.DropIndex(
                name: "IX_Appellation_RegionId",
                table: "Appellation");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Appellation");
        }
    }
}
