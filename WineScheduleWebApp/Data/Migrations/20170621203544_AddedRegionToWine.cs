using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WineScheduleWebApp.Data.Migrations
{
    public partial class AddedRegionToWine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Record_Wine_WineId",
                table: "Record");

            migrationBuilder.AddColumn<string>(
                name: "RegionId",
                table: "Wine",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WineId",
                table: "Record",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wine_RegionId",
                table: "Wine",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Wine_WineId",
                table: "Record",
                column: "WineId",
                principalTable: "Wine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wine_Region_RegionId",
                table: "Wine",
                column: "RegionId",
                principalTable: "Region",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Record_Wine_WineId",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Wine_Region_RegionId",
                table: "Wine");

            migrationBuilder.DropIndex(
                name: "IX_Wine_RegionId",
                table: "Wine");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Wine");

            migrationBuilder.AlterColumn<string>(
                name: "WineId",
                table: "Record",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Wine_WineId",
                table: "Record",
                column: "WineId",
                principalTable: "Wine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
