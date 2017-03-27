using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DementiaHelper.WebApi.Migrations
{
    public partial class UpdatedRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaregiverConnection_Caregiver_CaregiverForeignKeyCaregiverId",
                table: "CaregiverConnection");

            migrationBuilder.DropForeignKey(
                name: "FK_CaregiverConnection_Citizen_CitizenForeignKeyCitizenId",
                table: "CaregiverConnection");

            migrationBuilder.DropForeignKey(
                name: "FK_RelativeConnectiob_Citizen_CitizenForeignKeyCitizenId",
                table: "RelativeConnectiob");

            migrationBuilder.DropForeignKey(
                name: "FK_RelativeConnectiob_Relative_RelativeForeignKeyRelativeId",
                table: "RelativeConnectiob");

            migrationBuilder.RenameColumn(
                name: "RelativeForeignKeyRelativeId",
                table: "RelativeConnectiob",
                newName: "Relative");

            migrationBuilder.RenameColumn(
                name: "CitizenForeignKeyCitizenId",
                table: "RelativeConnectiob",
                newName: "Citizen");

            migrationBuilder.RenameIndex(
                name: "IX_RelativeConnectiob_RelativeForeignKeyRelativeId",
                table: "RelativeConnectiob",
                newName: "IX_RelativeConnectiob_Relative");

            migrationBuilder.RenameIndex(
                name: "IX_RelativeConnectiob_CitizenForeignKeyCitizenId",
                table: "RelativeConnectiob",
                newName: "IX_RelativeConnectiob_Citizen");

            migrationBuilder.RenameColumn(
                name: "CitizenForeignKeyCitizenId",
                table: "CaregiverConnection",
                newName: "Citizen");

            migrationBuilder.RenameColumn(
                name: "CaregiverForeignKeyCaregiverId",
                table: "CaregiverConnection",
                newName: "Caregiver");

            migrationBuilder.RenameIndex(
                name: "IX_CaregiverConnection_CitizenForeignKeyCitizenId",
                table: "CaregiverConnection",
                newName: "IX_CaregiverConnection_Citizen");

            migrationBuilder.RenameIndex(
                name: "IX_CaregiverConnection_CaregiverForeignKeyCaregiverId",
                table: "CaregiverConnection",
                newName: "IX_CaregiverConnection_Caregiver");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Relative",
                table: "RelativeConnectiob",
                newName: "RelativeForeignKeyRelativeId");

            migrationBuilder.RenameColumn(
                name: "Citizen",
                table: "RelativeConnectiob",
                newName: "CitizenForeignKeyCitizenId");

            migrationBuilder.RenameIndex(
                name: "IX_RelativeConnectiob_Relative",
                table: "RelativeConnectiob",
                newName: "IX_RelativeConnectiob_RelativeForeignKeyRelativeId");

            migrationBuilder.RenameIndex(
                name: "IX_RelativeConnectiob_Citizen",
                table: "RelativeConnectiob",
                newName: "IX_RelativeConnectiob_CitizenForeignKeyCitizenId");

            migrationBuilder.RenameColumn(
                name: "Citizen",
                table: "CaregiverConnection",
                newName: "CitizenForeignKeyCitizenId");

            migrationBuilder.RenameColumn(
                name: "Caregiver",
                table: "CaregiverConnection",
                newName: "CaregiverForeignKeyCaregiverId");

            migrationBuilder.RenameIndex(
                name: "IX_CaregiverConnection_Citizen",
                table: "CaregiverConnection",
                newName: "IX_CaregiverConnection_CitizenForeignKeyCitizenId");

            migrationBuilder.RenameIndex(
                name: "IX_CaregiverConnection_Caregiver",
                table: "CaregiverConnection",
                newName: "IX_CaregiverConnection_CaregiverForeignKeyCaregiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaregiverConnection_Caregiver_CaregiverForeignKeyCaregiverId",
                table: "CaregiverConnection",
                column: "CaregiverForeignKeyCaregiverId",
                principalTable: "Caregiver",
                principalColumn: "CaregiverId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaregiverConnection_Citizen_CitizenForeignKeyCitizenId",
                table: "CaregiverConnection",
                column: "CitizenForeignKeyCitizenId",
                principalTable: "Citizen",
                principalColumn: "CitizenId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelativeConnectiob_Citizen_CitizenForeignKeyCitizenId",
                table: "RelativeConnectiob",
                column: "CitizenForeignKeyCitizenId",
                principalTable: "Citizen",
                principalColumn: "CitizenId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelativeConnectiob_Relative_RelativeForeignKeyRelativeId",
                table: "RelativeConnectiob",
                column: "RelativeForeignKeyRelativeId",
                principalTable: "Relative",
                principalColumn: "RelativeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
