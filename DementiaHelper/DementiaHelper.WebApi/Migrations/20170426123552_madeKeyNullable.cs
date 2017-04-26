using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DementiaHelper.WebApi.Migrations
{
    public partial class madeKeyNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_ChatGroups_ChatGroupId",
                table: "ApplicationUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ChatGroupId",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_ChatGroups_ChatGroupId",
                table: "ApplicationUsers",
                column: "ChatGroupId",
                principalTable: "ChatGroups",
                principalColumn: "ChatGroupId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_ChatGroups_ChatGroupId",
                table: "ApplicationUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ChatGroupId",
                table: "ApplicationUsers",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_ChatGroups_ChatGroupId",
                table: "ApplicationUsers",
                column: "ChatGroupId",
                principalTable: "ChatGroups",
                principalColumn: "ChatGroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
