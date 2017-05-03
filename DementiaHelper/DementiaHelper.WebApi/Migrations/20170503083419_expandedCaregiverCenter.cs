using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DementiaHelper.WebApi.Migrations
{
    public partial class expandedCaregiverCenter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Caregivers_CaregiverCenter_CaregiverCenterId",
                table: "Caregivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Citizens_CaregiverCenter_CaregiverCenterId",
                table: "Citizens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaregiverCenter",
                table: "CaregiverCenter");

            migrationBuilder.RenameTable(
                name: "CaregiverCenter",
                newName: "CaregiverCenters");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CaregiverCenters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Phone",
                table: "CaregiverCenters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaregiverCenters",
                table: "CaregiverCenters",
                column: "CaregiverCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Caregivers_CaregiverCenters_CaregiverCenterId",
                table: "Caregivers",
                column: "CaregiverCenterId",
                principalTable: "CaregiverCenters",
                principalColumn: "CaregiverCenterId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Citizens_CaregiverCenters_CaregiverCenterId",
                table: "Citizens",
                column: "CaregiverCenterId",
                principalTable: "CaregiverCenters",
                principalColumn: "CaregiverCenterId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Caregivers_CaregiverCenters_CaregiverCenterId",
                table: "Caregivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Citizens_CaregiverCenters_CaregiverCenterId",
                table: "Citizens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaregiverCenters",
                table: "CaregiverCenters");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CaregiverCenters");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "CaregiverCenters");

            migrationBuilder.RenameTable(
                name: "CaregiverCenters",
                newName: "CaregiverCenter");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaregiverCenter",
                table: "CaregiverCenter",
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
    }
}
