using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DementiaHelper.WebApi.Migrations
{
    public partial class RefactoredDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Role_RoleId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Citizen_CitizenId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Caregiver_ApplicationUsers_CaregiverId",
                table: "Caregiver");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_ChatGroups_ChatGroupId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Citizen_ApplicationUsers_CitizenId",
                table: "Citizen");

            migrationBuilder.DropForeignKey(
                name: "FK_Relative_ApplicationUsers_RelativeId",
                table: "Relative");

            migrationBuilder.DropTable(
                name: "AccountPictures");

            migrationBuilder.DropTable(
                name: "ShoppingListDetails");

            migrationBuilder.DropTable(
                name: "AccountInformations");

            migrationBuilder.DropTable(
                name: "ShoppingLists");

            migrationBuilder.DropTable(
                name: "CaregiverConnection");

            migrationBuilder.DropTable(
                name: "RelativeConnectiob");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Relative",
                table: "Relative");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Citizen",
                table: "Citizen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Caregiver",
                table: "Caregiver");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Relative",
                newName: "Relatives");

            migrationBuilder.RenameTable(
                name: "Citizen",
                newName: "Citizens");

            migrationBuilder.RenameTable(
                name: "Caregiver",
                newName: "Caregivers");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "Roles",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "CitizenId",
                table: "Relatives",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Products",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "CaregiverId",
                table: "Citizens",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConnectionId",
                table: "Citizens",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "ChatGroups",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "ConnectionId",
                table: "Caregivers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Appointments",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "ApplicationUsers",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "ApplicationUsers",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ApplicationUsers",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relatives",
                table: "Relatives",
                column: "RelativeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Citizens",
                table: "Citizens",
                column: "CitizenId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Caregivers",
                table: "Caregivers",
                column: "CaregiverId");

            migrationBuilder.CreateTable(
                name: "ShoppingListItems",
                columns: table => new
                {
                    ShoppingListItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bought = table.Column<bool>(nullable: false),
                    CitizenId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListItems", x => x.ShoppingListItemId);
                    table.ForeignKey(
                        name: "FK_ShoppingListItems_Citizens_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizens",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingListItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    ChatMessageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChatGroupId = table.Column<int>(nullable: false),
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
                name: "IX_Relatives_CitizenId",
                table: "Relatives",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_CaregiverId",
                table: "Citizens",
                column: "CaregiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_CitizenId",
                table: "ShoppingListItems",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_ProductId",
                table: "ShoppingListItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Roles_RoleId",
                table: "ApplicationUsers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Citizens_CitizenId",
                table: "Appointments",
                column: "CitizenId",
                principalTable: "Citizens",
                principalColumn: "CitizenId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Caregivers_ApplicationUsers_CaregiverId",
                table: "Caregivers",
                column: "CaregiverId",
                principalTable: "ApplicationUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Citizens_Caregivers_CaregiverId",
                table: "Citizens",
                column: "CaregiverId",
                principalTable: "Caregivers",
                principalColumn: "CaregiverId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Citizens_ApplicationUsers_CitizenId",
                table: "Citizens",
                column: "CitizenId",
                principalTable: "ApplicationUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relatives_Citizens_CitizenId",
                table: "Relatives",
                column: "CitizenId",
                principalTable: "Citizens",
                principalColumn: "CitizenId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Relatives_ApplicationUsers_RelativeId",
                table: "Relatives",
                column: "RelativeId",
                principalTable: "ApplicationUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Roles_RoleId",
                table: "ApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Citizens_CitizenId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Caregivers_ApplicationUsers_CaregiverId",
                table: "Caregivers");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_ChatGroups_ChatGroupId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Citizens_Caregivers_CaregiverId",
                table: "Citizens");

            migrationBuilder.DropForeignKey(
                name: "FK_Citizens_ApplicationUsers_CitizenId",
                table: "Citizens");

            migrationBuilder.DropForeignKey(
                name: "FK_Relatives_Citizens_CitizenId",
                table: "Relatives");

            migrationBuilder.DropForeignKey(
                name: "FK_Relatives_ApplicationUsers_RelativeId",
                table: "Relatives");

            migrationBuilder.DropTable(
                name: "ShoppingListItems");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Relatives",
                table: "Relatives");

            migrationBuilder.DropIndex(
                name: "IX_Relatives_CitizenId",
                table: "Relatives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Citizens",
                table: "Citizens");

            migrationBuilder.DropIndex(
                name: "IX_Citizens_CaregiverId",
                table: "Citizens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Caregivers",
                table: "Caregivers");

            migrationBuilder.DropColumn(
                name: "CitizenId",
                table: "Relatives");

            migrationBuilder.DropColumn(
                name: "CaregiverId",
                table: "Citizens");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Citizens");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Caregivers");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "Relatives",
                newName: "Relative");

            migrationBuilder.RenameTable(
                name: "Citizens",
                newName: "Citizen");

            migrationBuilder.RenameTable(
                name: "Caregivers",
                newName: "Caregiver");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "Role",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Products",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "ChatGroups",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Appointments",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Salt",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relative",
                table: "Relative",
                column: "RelativeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Citizen",
                table: "Citizen",
                column: "CitizenId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Caregiver",
                table: "Caregiver",
                column: "CaregiverId");

            migrationBuilder.CreateTable(
                name: "AccountInformations",
                columns: table => new
                {
                    AccountInformationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountInformations", x => x.AccountInformationId);
                });

            migrationBuilder.CreateTable(
                name: "CaregiverConnection",
                columns: table => new
                {
                    CaregiverConnectionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaregiverId = table.Column<int>(nullable: false),
                    CitizenId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaregiverConnection", x => x.CaregiverConnectionId);
                    table.ForeignKey(
                        name: "FK_CaregiverConnection_Caregiver_CaregiverId",
                        column: x => x.CaregiverId,
                        principalTable: "Caregiver",
                        principalColumn: "CaregiverId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaregiverConnection_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelativeConnectiob",
                columns: table => new
                {
                    RelativeConnectionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CitizenId = table.Column<int>(nullable: false),
                    RelativeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelativeConnectiob", x => x.RelativeConnectionId);
                    table.ForeignKey(
                        name: "FK_RelativeConnectiob_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelativeConnectiob_Relative_RelativeId",
                        column: x => x.RelativeId,
                        principalTable: "Relative",
                        principalColumn: "RelativeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountPictures",
                columns: table => new
                {
                    AccountPictureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountInformationForeignKey = table.Column<int>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPictures", x => x.AccountPictureId);
                    table.ForeignKey(
                        name: "FK_AccountPictures_AccountInformations_AccountInformationForeignKey",
                        column: x => x.AccountInformationForeignKey,
                        principalTable: "AccountInformations",
                        principalColumn: "AccountInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingLists",
                columns: table => new
                {
                    ShoppingListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaregiverConnectionId = table.Column<int>(nullable: false),
                    RelativeConnectionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingLists", x => x.ShoppingListId);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_CaregiverConnection_CaregiverConnectionId",
                        column: x => x.CaregiverConnectionId,
                        principalTable: "CaregiverConnection",
                        principalColumn: "CaregiverConnectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_RelativeConnectiob_RelativeConnectionId",
                        column: x => x.RelativeConnectionId,
                        principalTable: "RelativeConnectiob",
                        principalColumn: "RelativeConnectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListDetails",
                columns: table => new
                {
                    ShoppingListDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bought = table.Column<bool>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ShoppingListId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListDetails", x => x.ShoppingListDetailId);
                    table.ForeignKey(
                        name: "FK_ShoppingListDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingListDetails_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "ShoppingListId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_AccountPictures_AccountInformationForeignKey",
                table: "AccountPictures",
                column: "AccountInformationForeignKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaregiverConnection_CaregiverId",
                table: "CaregiverConnection",
                column: "CaregiverId");

            migrationBuilder.CreateIndex(
                name: "IX_CaregiverConnection_CitizenId",
                table: "CaregiverConnection",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_RelativeConnectiob_CitizenId",
                table: "RelativeConnectiob",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_RelativeConnectiob_RelativeId",
                table: "RelativeConnectiob",
                column: "RelativeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_CaregiverConnectionId",
                table: "ShoppingLists",
                column: "CaregiverConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_RelativeConnectionId",
                table: "ShoppingLists",
                column: "RelativeConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListDetails_ProductId",
                table: "ShoppingListDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListDetails_ShoppingListId",
                table: "ShoppingListDetails",
                column: "ShoppingListId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Role_RoleId",
                table: "ApplicationUsers",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Citizen_CitizenId",
                table: "Appointments",
                column: "CitizenId",
                principalTable: "Citizen",
                principalColumn: "CitizenId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Caregiver_ApplicationUsers_CaregiverId",
                table: "Caregiver",
                column: "CaregiverId",
                principalTable: "ApplicationUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_ChatGroups_ChatGroupId",
                table: "ChatMessages",
                column: "ChatGroupId",
                principalTable: "ChatGroups",
                principalColumn: "ChatGroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Citizen_ApplicationUsers_CitizenId",
                table: "Citizen",
                column: "CitizenId",
                principalTable: "ApplicationUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Relative_ApplicationUsers_RelativeId",
                table: "Relative",
                column: "RelativeId",
                principalTable: "ApplicationUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
