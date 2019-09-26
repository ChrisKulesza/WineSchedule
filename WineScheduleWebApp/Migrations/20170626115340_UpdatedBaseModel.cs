using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WineScheduleWebApp.Migrations
{
    public partial class UpdatedBaseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "WineGrape",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Wine",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Region",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Record",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Grape",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Dryness",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Dryness",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "Dryness",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Category",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "Category",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Appellation",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WineGrape_ApplicationUserId",
                table: "WineGrape",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wine_ApplicationUserId",
                table: "Wine",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_ApplicationUserId",
                table: "Region",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Record_ApplicationUserId",
                table: "Record",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Grape_ApplicationUserId",
                table: "Grape",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dryness_ApplicationUserId",
                table: "Dryness",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ApplicationUserId",
                table: "Category",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Appellation_ApplicationUserId",
                table: "Appellation",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appellation_AspNetUsers_ApplicationUserId",
                table: "Appellation",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_AspNetUsers_ApplicationUserId",
                table: "Category",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dryness_AspNetUsers_ApplicationUserId",
                table: "Dryness",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grape_AspNetUsers_ApplicationUserId",
                table: "Grape",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Record_AspNetUsers_ApplicationUserId",
                table: "Record",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Region_AspNetUsers_ApplicationUserId",
                table: "Region",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wine_AspNetUsers_ApplicationUserId",
                table: "Wine",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WineGrape_AspNetUsers_ApplicationUserId",
                table: "WineGrape",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appellation_AspNetUsers_ApplicationUserId",
                table: "Appellation");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_AspNetUsers_ApplicationUserId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Dryness_AspNetUsers_ApplicationUserId",
                table: "Dryness");

            migrationBuilder.DropForeignKey(
                name: "FK_Grape_AspNetUsers_ApplicationUserId",
                table: "Grape");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_AspNetUsers_ApplicationUserId",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_AspNetUsers_ApplicationUserId",
                table: "Region");

            migrationBuilder.DropForeignKey(
                name: "FK_Wine_AspNetUsers_ApplicationUserId",
                table: "Wine");

            migrationBuilder.DropForeignKey(
                name: "FK_WineGrape_AspNetUsers_ApplicationUserId",
                table: "WineGrape");

            migrationBuilder.DropIndex(
                name: "IX_WineGrape_ApplicationUserId",
                table: "WineGrape");

            migrationBuilder.DropIndex(
                name: "IX_Wine_ApplicationUserId",
                table: "Wine");

            migrationBuilder.DropIndex(
                name: "IX_Region_ApplicationUserId",
                table: "Region");

            migrationBuilder.DropIndex(
                name: "IX_Record_ApplicationUserId",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Grape_ApplicationUserId",
                table: "Grape");

            migrationBuilder.DropIndex(
                name: "IX_Dryness_ApplicationUserId",
                table: "Dryness");

            migrationBuilder.DropIndex(
                name: "IX_Category_ApplicationUserId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Appellation_ApplicationUserId",
                table: "Appellation");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Dryness");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Dryness");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Dryness");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Category");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "WineGrape",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Wine",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Region",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Record",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Grape",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Appellation",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
