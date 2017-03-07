using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DementiaHelper.WebApi.Migrations
{
    public partial class addedaccountinformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BorgerId",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_AccountPictures_AccountInformationForeignKey",
                table: "AccountPictures",
                column: "AccountInformationForeignKey",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountPictures");

            migrationBuilder.DropTable(
                name: "AccountInformations");

            migrationBuilder.DropColumn(
                name: "BorgerId",
                table: "ApplicationUsers");
        }
    }
}
