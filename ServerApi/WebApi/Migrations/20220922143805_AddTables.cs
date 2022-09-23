using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerApi.Migrations
{
    public partial class AddTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bayer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: false),
                    UpdateOn = table.Column<DateTime>(nullable: false),
                    DeleteOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bayer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: false),
                    UpdateOn = table.Column<DateTime>(nullable: false),
                    DeleteOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: false),
                    UpdateOn = table.Column<DateTime>(nullable: false),
                    DeleteOn = table.Column<DateTime>(nullable: true),
                    Date = table.Column<DateTime>(type: "date", unicode: false, nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", unicode: false, nullable: false),
                    BuyerId = table.Column<int>(nullable: true),
                    TotalAmount = table.Column<int>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sale_Bayer_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Bayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProvidedProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: false),
                    UpdateOn = table.Column<DateTime>(nullable: false),
                    DeleteOn = table.Column<DateTime>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ProductQuantity = table.Column<int>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvidedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProvidedProducts_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: false),
                    UpdateOn = table.Column<DateTime>(nullable: false),
                    DeleteOn = table.Column<DateTime>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ProductQuantity = table.Column<int>(unicode: false, nullable: false),
                    ProductIdAmount = table.Column<int>(unicode: false, nullable: false),
                    SaleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesData_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesData_Sale_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesPoint",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreateOn = table.Column<DateTime>(nullable: false),
                    UpdateOn = table.Column<DateTime>(nullable: false),
                    DeleteOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(type: "varchar(256)", unicode: false, nullable: false),
                    ProvidedProductsId = table.Column<int>(nullable: false),
                    SaleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesPoint_ProvidedProducts_ProvidedProductsId",
                        column: x => x.ProvidedProductsId,
                        principalTable: "ProvidedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesPoint_Sale_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProvidedProducts_ProductId",
                table: "ProvidedProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_BuyerId",
                table: "Sale",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesData_ProductId",
                table: "SalesData",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesData_SaleId",
                table: "SalesData",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPoint_ProvidedProductsId",
                table: "SalesPoint",
                column: "ProvidedProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPoint_SaleId",
                table: "SalesPoint",
                column: "SaleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesData");

            migrationBuilder.DropTable(
                name: "SalesPoint");

            migrationBuilder.DropTable(
                name: "ProvidedProducts");

            migrationBuilder.DropTable(
                name: "Sale");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Bayer");
        }
    }
}
