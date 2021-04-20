using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KURSOVA.Migrations
{
    public partial class AddStatusesToOrderAndDelivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.AddColumn<int>(
               name: "Status",
               table: "Order",
               nullable: true,
               defaultValue: null);

            migrationBuilder.AddColumn<int>(
                name: "Status", 
                table: "Delivery",
                nullable: true,
                defaultValue: null); 
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Status", "Delivery");
            migrationBuilder.DropColumn("Status", "Order");

        }
    }
}
