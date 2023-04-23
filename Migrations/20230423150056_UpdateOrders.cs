using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Northwind.Migrations
{
    public partial class UpdateOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippedVia",
                table: "Orders",
                newName: "ShipVia");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShipVia",
                table: "Orders",
                newName: "ShippedVia");
        }
    }
}
