using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WineScheduleWebApp.Migrations
{
    public partial class AddedRelationshipBetweenGrapeAndCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Grape",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Dryness",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grape_CategoryId",
                table: "Grape",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grape_Category_CategoryId",
                table: "Grape",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grape_Category_CategoryId",
                table: "Grape");

            migrationBuilder.DropIndex(
                name: "IX_Grape_CategoryId",
                table: "Grape");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Grape");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Dryness",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
