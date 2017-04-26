using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DementiaHelper.WebApi.Migrations
{
    public partial class AddedChatTodatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaregiverConnection_Caregiver_Caregiver",
                table: "CaregiverConnection");

            migrationBuilder.DropForeignKey(
                name: "FK_CaregiverConnection_Citizen_Citizen",
                table: "CaregiverConnection");

            migrationBuilder.DropForeignKey(
                name: "FK_RelativeConnectiob_Citizen_Citizen",
                table: "RelativeConnectiob");

            migrationBuilder.DropForeignKey(
                name: "FK_RelativeConnectiob_Relative_Relative",
                table: "RelativeConnectiob");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingLists_CaregiverConnection_CaregiverConnection",
                table: "ShoppingLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingLists_RelativeConnectiob_RelativeConnection",
                table: "ShoppingLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListDetails_Products_Product",
                table: "ShoppingListDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListDetails_ShoppingLists_ShoppingList",
                table: "ShoppingListDetails");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingListDetails_Product",
                table: "ShoppingListDetails");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingListDetails_ShoppingList",
                table: "ShoppingListDetails");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingLists_CaregiverConnection",
                table: "ShoppingLists");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingLists_RelativeConnection",
                table: "ShoppingLists");

            migrationBuilder.DropIndex(
                name: "IX_RelativeConnectiob_Citizen",
                table: "RelativeConnectiob");

            migrationBuilder.DropIndex(
                name: "IX_RelativeConnectiob_Relative",
                table: "RelativeConnectiob");

            migrationBuilder.DropIndex(
                name: "IX_CaregiverConnection_Caregiver",
                table: "CaregiverConnection");

            migrationBuilder.DropIndex(
                name: "IX_CaregiverConnection_Citizen",
                table: "CaregiverConnection");

            migrationBuilder.DropColumn(
                name: "Product",
                table: "ShoppingListDetails");

            migrationBuilder.DropColumn(
                name: "ShoppingList",
                table: "ShoppingListDetails");

            migrationBuilder.DropColumn(
                name: "CaregiverConnection",
                table: "ShoppingLists");

            migrationBuilder.DropColumn(
                name: "RelativeConnection",
                table: "ShoppingLists");

            migrationBuilder.DropColumn(
                name: "Citizen",
                table: "RelativeConnectiob");

            migrationBuilder.DropColumn(
                name: "Relative",
                table: "RelativeConnectiob");

            migrationBuilder.DropColumn(
                name: "ApplicationUserForeignKey",
                table: "Relative");

            migrationBuilder.DropColumn(
                name: "ApplicationUserForeignKey",
                table: "Citizen");

            migrationBuilder.DropColumn(
                name: "Caregiver",
                table: "CaregiverConnection");

            migrationBuilder.DropColumn(
                name: "Citizen",
                table: "CaregiverConnection");

            migrationBuilder.DropColumn(
                name: "ApplicationUserForeignKey",
                table: "Caregiver");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ShoppingListDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShoppingListId",
                table: "ShoppingListDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CaregiverConnectionId",
                table: "ShoppingLists",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RelativeConnectionId",
                table: "ShoppingLists",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CitizenId",
                table: "RelativeConnectiob",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RelativeId",
                table: "RelativeConnectiob",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CaregiverId",
                table: "CaregiverConnection",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CitizenId",
                table: "CaregiverConnection",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChatGroupId",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChatGroups",
                columns: table => new
                {
                    ChatGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroups", x => x.ChatGroupId);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    ChatMessageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChatGroupId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Sender = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.ChatMessageId);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatGroups_ChatGroupId",
                        column: x => x.ChatGroupId,
                        principalTable: "ChatGroups",
                        principalColumn: "ChatGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListDetails_ProductId",
                table: "ShoppingListDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListDetails_ShoppingListId",
                table: "ShoppingListDetails",
                column: "ShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_CaregiverConnectionId",
                table: "ShoppingLists",
                column: "CaregiverConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_RelativeConnectionId",
                table: "ShoppingLists",
                column: "RelativeConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_RelativeConnectiob_CitizenId",
                table: "RelativeConnectiob",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_RelativeConnectiob_RelativeId",
                table: "RelativeConnectiob",
                column: "RelativeId");

            migrationBuilder.CreateIndex(
                name: "IX_CaregiverConnection_CaregiverId",
                table: "CaregiverConnection",
                column: "CaregiverId");

            migrationBuilder.CreateIndex(
                name: "IX_CaregiverConnection_CitizenId",
                table: "CaregiverConnection",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_ChatGroupId",
                table: "ApplicationUsers",
                column: "ChatGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatGroupId",
                table: "ChatMessages",
                column: "ChatGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_ChatGroups_ChatGroupId",
                table: "ApplicationUsers",
                column: "ChatGroupId",
                principalTable: "ChatGroups",
                principalColumn: "ChatGroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaregiverConnection_Caregiver_CaregiverId",
                table: "CaregiverConnection",
                column: "CaregiverId",
                principalTable: "Caregiver",
                principalColumn: "CaregiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaregiverConnection_Citizen_CitizenId",
                table: "CaregiverConnection",
                column: "CitizenId",
                principalTable: "Citizen",
                principalColumn: "CitizenId");

            migrationBuilder.AddForeignKey(
                name: "FK_RelativeConnectiob_Citizen_CitizenId",
                table: "RelativeConnectiob",
                column: "CitizenId",
                principalTable: "Citizen",
                principalColumn: "CitizenId");

            migrationBuilder.AddForeignKey(
                name: "FK_RelativeConnectiob_Relative_RelativeId",
                table: "RelativeConnectiob",
                column: "RelativeId",
                principalTable: "Relative",
                principalColumn: "RelativeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingLists_CaregiverConnection_CaregiverConnectionId",
                table: "ShoppingLists",
                column: "CaregiverConnectionId",
                principalTable: "CaregiverConnection",
                principalColumn: "CaregiverConnectionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingLists_RelativeConnectiob_RelativeConnectionId",
                table: "ShoppingLists",
                column: "RelativeConnectionId",
                principalTable: "RelativeConnectiob",
                principalColumn: "RelativeConnectionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListDetails_Products_ProductId",
                table: "ShoppingListDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListDetails_ShoppingLists_ShoppingListId",
                table: "ShoppingListDetails",
                column: "ShoppingListId",
                principalTable: "ShoppingLists",
                principalColumn: "ShoppingListId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_ChatGroups_ChatGroupId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CaregiverConnection_Caregiver_CaregiverId",
                table: "CaregiverConnection");

            migrationBuilder.DropForeignKey(
                name: "FK_CaregiverConnection_Citizen_CitizenId",
                table: "CaregiverConnection");

            migrationBuilder.DropForeignKey(
                name: "FK_RelativeConnectiob_Citizen_CitizenId",
                table: "RelativeConnectiob");

            migrationBuilder.DropForeignKey(
                name: "FK_RelativeConnectiob_Relative_RelativeId",
                table: "RelativeConnectiob");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingLists_CaregiverConnection_CaregiverConnectionId",
                table: "ShoppingLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingLists_RelativeConnectiob_RelativeConnectionId",
                table: "ShoppingLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListDetails_Products_ProductId",
                table: "ShoppingListDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListDetails_ShoppingLists_ShoppingListId",
                table: "ShoppingListDetails");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ChatGroups");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingListDetails_ProductId",
                table: "ShoppingListDetails");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingListDetails_ShoppingListId",
                table: "ShoppingListDetails");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingLists_CaregiverConnectionId",
                table: "ShoppingLists");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingLists_RelativeConnectionId",
                table: "ShoppingLists");

            migrationBuilder.DropIndex(
                name: "IX_RelativeConnectiob_CitizenId",
                table: "RelativeConnectiob");

            migrationBuilder.DropIndex(
                name: "IX_RelativeConnectiob_RelativeId",
                table: "RelativeConnectiob");

            migrationBuilder.DropIndex(
                name: "IX_CaregiverConnection_CaregiverId",
                table: "CaregiverConnection");

            migrationBuilder.DropIndex(
                name: "IX_CaregiverConnection_CitizenId",
                table: "CaregiverConnection");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_ChatGroupId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ShoppingListDetails");

            migrationBuilder.DropColumn(
                name: "ShoppingListId",
                table: "ShoppingListDetails");

            migrationBuilder.DropColumn(
                name: "CaregiverConnectionId",
                table: "ShoppingLists");

            migrationBuilder.DropColumn(
                name: "RelativeConnectionId",
                table: "ShoppingLists");

            migrationBuilder.DropColumn(
                name: "CitizenId",
                table: "RelativeConnectiob");

            migrationBuilder.DropColumn(
                name: "RelativeId",
                table: "RelativeConnectiob");

            migrationBuilder.DropColumn(
                name: "CaregiverId",
                table: "CaregiverConnection");

            migrationBuilder.DropColumn(
                name: "CitizenId",
                table: "CaregiverConnection");

            migrationBuilder.DropColumn(
                name: "ChatGroupId",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<int>(
                name: "Product",
                table: "ShoppingListDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShoppingList",
                table: "ShoppingListDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CaregiverConnection",
                table: "ShoppingLists",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelativeConnection",
                table: "ShoppingLists",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Citizen",
                table: "RelativeConnectiob",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Relative",
                table: "RelativeConnectiob",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserForeignKey",
                table: "Relative",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserForeignKey",
                table: "Citizen",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Caregiver",
                table: "CaregiverConnection",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Citizen",
                table: "CaregiverConnection",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserForeignKey",
                table: "Caregiver",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListDetails_Product",
                table: "ShoppingListDetails",
                column: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListDetails_ShoppingList",
                table: "ShoppingListDetails",
                column: "ShoppingList");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_CaregiverConnection",
                table: "ShoppingLists",
                column: "CaregiverConnection");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_RelativeConnection",
                table: "ShoppingLists",
                column: "RelativeConnection");

            migrationBuilder.CreateIndex(
                name: "IX_RelativeConnectiob_Citizen",
                table: "RelativeConnectiob",
                column: "Citizen");

            migrationBuilder.CreateIndex(
                name: "IX_RelativeConnectiob_Relative",
                table: "RelativeConnectiob",
                column: "Relative");

            migrationBuilder.CreateIndex(
                name: "IX_CaregiverConnection_Caregiver",
                table: "CaregiverConnection",
                column: "Caregiver");

            migrationBuilder.CreateIndex(
                name: "IX_CaregiverConnection_Citizen",
                table: "CaregiverConnection",
                column: "Citizen");

            migrationBuilder.AddForeignKey(
                name: "FK_CaregiverConnection_Caregiver_Caregiver",
                table: "CaregiverConnection",
                column: "Caregiver",
                principalTable: "Caregiver",
                principalColumn: "CaregiverId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaregiverConnection_Citizen_Citizen",
                table: "CaregiverConnection",
                column: "Citizen",
                principalTable: "Citizen",
                principalColumn: "CitizenId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelativeConnectiob_Citizen_Citizen",
                table: "RelativeConnectiob",
                column: "Citizen",
                principalTable: "Citizen",
                principalColumn: "CitizenId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelativeConnectiob_Relative_Relative",
                table: "RelativeConnectiob",
                column: "Relative",
                principalTable: "Relative",
                principalColumn: "RelativeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingLists_CaregiverConnection_CaregiverConnection",
                table: "ShoppingLists",
                column: "CaregiverConnection",
                principalTable: "CaregiverConnection",
                principalColumn: "CaregiverConnectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingLists_RelativeConnectiob_RelativeConnection",
                table: "ShoppingLists",
                column: "RelativeConnection",
                principalTable: "RelativeConnectiob",
                principalColumn: "RelativeConnectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListDetails_Products_Product",
                table: "ShoppingListDetails",
                column: "Product",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListDetails_ShoppingLists_ShoppingList",
                table: "ShoppingListDetails",
                column: "ShoppingList",
                principalTable: "ShoppingLists",
                principalColumn: "ShoppingListId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
