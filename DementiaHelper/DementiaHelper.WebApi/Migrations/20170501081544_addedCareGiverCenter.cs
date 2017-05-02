using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DementiaHelper.WebApi.Migrations
{
    public partial class addedCareGiverCenter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citizens_Caregivers_CaregiverId",
                table: "Citizens");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Caregivers");

            migrationBuilder.RenameColumn(
                name: "CaregiverId",
                table: "Citizens",
                newName: "CaregiverCenterId");

            migrationBuilder.RenameIndex(
                name: "IX_Citizens_CaregiverId",
                table: "Citizens",
                newName: "IX_Citizens_CaregiverCenterId");

            migrationBuilder.AddColumn<int>(
                name: "CaregiverCenterId",
                table: "Caregivers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Lastname",
                table: "ApplicationUsers",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "ApplicationUsers",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "CaregiverCenter",
                columns: table => new
                {
                    CaregiverCenterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaregiverConnectionId = table.Column<string>(nullable: false),
                    CitizenConnectionId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaregiverCenter", x => x.CaregiverCenterId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Caregivers_CaregiverCenterId",
                table: "Caregivers",
                column: "CaregiverCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Caregivers_CaregiverCenter_CaregiverCenterId",
                table: "Caregivers",
                column: "CaregiverCenterId",
                principalTable: "CaregiverCenter",
                principalColumn: "CaregiverCenterId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Citizens_CaregiverCenter_CaregiverCenterId",
                table: "Citizens",
                column: "CaregiverCenterId",
                principalTable: "CaregiverCenter",
                principalColumn: "CaregiverCenterId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Caregivers_CaregiverCenter_CaregiverCenterId",
                table: "Caregivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Citizens_CaregiverCenter_CaregiverCenterId",
                table: "Citizens");

            migrationBuilder.DropTable(
                name: "CaregiverCenter");

            migrationBuilder.DropIndex(
                name: "IX_Caregivers_CaregiverCenterId",
                table: "Caregivers");

            migrationBuilder.DropColumn(
                name: "CaregiverCenterId",
                table: "Caregivers");

            migrationBuilder.RenameColumn(
                name: "CaregiverCenterId",
                table: "Citizens",
                newName: "CaregiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Citizens_CaregiverCenterId",
                table: "Citizens",
                newName: "IX_Citizens_CaregiverId");

            migrationBuilder.AddColumn<int>(
                name: "ConnectionId",
                table: "Caregivers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Lastname",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Citizens_Caregivers_CaregiverId",
                table: "Citizens",
                column: "CaregiverId",
                principalTable: "Caregivers",
                principalColumn: "CaregiverId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
