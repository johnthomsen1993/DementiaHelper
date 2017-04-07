using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DementiaHelper.WebApi.Migrations
{
    public partial class AddShoppingList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingLists",
                columns: table => new
                {
                    ShoppingListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CaregiverConnection = table.Column<int>(nullable: true),
                    RelativeConnection = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingLists", x => x.ShoppingListId);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_CaregiverConnection_CaregiverConnection",
                        column: x => x.CaregiverConnection,
                        principalTable: "CaregiverConnection",
                        principalColumn: "CaregiverConnectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingLists_RelativeConnectiob_RelativeConnection",
                        column: x => x.RelativeConnection,
                        principalTable: "RelativeConnectiob",
                        principalColumn: "RelativeConnectionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListDetails",
                columns: table => new
                {
                    ShoppingListDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bought = table.Column<bool>(nullable: false),
                    Product = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    ShoppingList = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListDetails", x => x.ShoppingListDetailId);
                    table.ForeignKey(
                        name: "FK_ShoppingListDetails_Products_Product",
                        column: x => x.Product,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShoppingListDetails_ShoppingLists_ShoppingList",
                        column: x => x.ShoppingList,
                        principalTable: "ShoppingLists",
                        principalColumn: "ShoppingListId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_CaregiverConnection",
                table: "ShoppingLists",
                column: "CaregiverConnection");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_RelativeConnection",
                table: "ShoppingLists",
                column: "RelativeConnection");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListDetails_Product",
                table: "ShoppingListDetails",
                column: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListDetails_ShoppingList",
                table: "ShoppingListDetails",
                column: "ShoppingList");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingListDetails");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ShoppingLists");
        }
    }
}
