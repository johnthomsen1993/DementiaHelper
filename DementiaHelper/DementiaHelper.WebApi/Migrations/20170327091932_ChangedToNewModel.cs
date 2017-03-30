using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DementiaHelper.WebApi.Migrations
{
    public partial class ChangedToNewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BorgerId",
                table: "ApplicationUsers",
                newName: "RoleId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lastname",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Caregiver",
                columns: table => new
                {
                    CaregiverId = table.Column<int>(nullable: false),
                    ApplicationUserForeignKey = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caregiver", x => x.CaregiverId);
                    table.ForeignKey(
                        name: "FK_Caregiver_ApplicationUsers_CaregiverId",
                        column: x => x.CaregiverId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "ApplicationUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Citizen",
                columns: table => new
                {
                    CitizenId = table.Column<int>(nullable: false),
                    ApplicationUserForeignKey = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizen", x => x.CitizenId);
                    table.ForeignKey(
                        name: "FK_Citizen_ApplicationUsers_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "ApplicationUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relative",
                columns: table => new
                {
                    RelativeId = table.Column<int>(nullable: false),
                    ApplicationUserForeignKey = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relative", x => x.RelativeId);
                    table.ForeignKey(
                        name: "FK_Relative_ApplicationUsers_RelativeId",
                        column: x => x.RelativeId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "ApplicationUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "CaregiverConnection",
                columns: table => new
                {
                    CaregiverConnectionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaregiverForeignKeyCaregiverId = table.Column<int>(nullable: true),
                    CitizenForeignKeyCitizenId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaregiverConnection", x => x.CaregiverConnectionId);
                    table.ForeignKey(
                        name: "FK_CaregiverConnection_Caregiver_CaregiverForeignKeyCaregiverId",
                        column: x => x.CaregiverForeignKeyCaregiverId,
                        principalTable: "Caregiver",
                        principalColumn: "CaregiverId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaregiverConnection_Citizen_CitizenForeignKeyCitizenId",
                        column: x => x.CitizenForeignKeyCitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RelativeConnectiob",
                columns: table => new
                {
                    RelativeConnectionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CitizenForeignKeyCitizenId = table.Column<int>(nullable: true),
                    RelativeForeignKeyRelativeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelativeConnectiob", x => x.RelativeConnectionId);
                    table.ForeignKey(
                        name: "FK_RelativeConnectiob_Citizen_CitizenForeignKeyCitizenId",
                        column: x => x.CitizenForeignKeyCitizenId,
                        principalTable: "Citizen",
                        principalColumn: "CitizenId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RelativeConnectiob_Relative_RelativeForeignKeyRelativeId",
                        column: x => x.RelativeForeignKeyRelativeId,
                        principalTable: "Relative",
                        principalColumn: "RelativeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_RoleId",
                table: "ApplicationUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CaregiverConnection_CaregiverForeignKeyCaregiverId",
                table: "CaregiverConnection",
                column: "CaregiverForeignKeyCaregiverId");

            migrationBuilder.CreateIndex(
                name: "IX_CaregiverConnection_CitizenForeignKeyCitizenId",
                table: "CaregiverConnection",
                column: "CitizenForeignKeyCitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_RelativeConnectiob_CitizenForeignKeyCitizenId",
                table: "RelativeConnectiob",
                column: "CitizenForeignKeyCitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_RelativeConnectiob_RelativeForeignKeyRelativeId",
                table: "RelativeConnectiob",
                column: "RelativeForeignKeyRelativeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Role_RoleId",
                table: "ApplicationUsers",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Role_RoleId",
                table: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "CaregiverConnection");

            migrationBuilder.DropTable(
                name: "RelativeConnectiob");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Caregiver");

            migrationBuilder.DropTable(
                name: "Citizen");

            migrationBuilder.DropTable(
                name: "Relative");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_RoleId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Lastname",
                table: "ApplicationUsers");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "ApplicationUsers",
                newName: "BorgerId");
        }
    }
}
