using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DementiaHelper.WebApi.Migrations
{
    public partial class addedChatGroupConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_ChatGroups_ChatGroupId",
                table: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_ChatGroupId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "ChatGroupId",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<int>(
                name: "GroupRole",
                table: "ChatGroups",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatGroupConnections",
                columns: table => new
                {
                    ChatGroupConnectionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<int>(nullable: false),
                    ChatGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroupConnections", x => x.ChatGroupConnectionId);
                    table.ForeignKey(
                        name: "FK_ChatGroupConnections_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "ApplicationUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatGroupConnections_ChatGroups_ChatGroupId",
                        column: x => x.ChatGroupId,
                        principalTable: "ChatGroups",
                        principalColumn: "ChatGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroupConnections_ApplicationUserId",
                table: "ChatGroupConnections",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroupConnections_ChatGroupId",
                table: "ChatGroupConnections",
                column: "ChatGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatGroupConnections");

            migrationBuilder.DropColumn(
                name: "GroupRole",
                table: "ChatGroups");

            migrationBuilder.AddColumn<int>(
                name: "ChatGroupId",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_ChatGroupId",
                table: "ApplicationUsers",
                column: "ChatGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_ChatGroups_ChatGroupId",
                table: "ApplicationUsers",
                column: "ChatGroupId",
                principalTable: "ChatGroups",
                principalColumn: "ChatGroupId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
