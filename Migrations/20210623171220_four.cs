using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryBookingCllient.Migrations
{
    public partial class four : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryStatus",
                columns: table => new
                {
                    DeliveryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    RequestID = table.Column<int>(type: "int", nullable: false),
                    ExecutiveID = table.Column<int>(type: "int", nullable: false),
                    Received = table.Column<bool>(type: "bit", nullable: false),
                    Shipped = table.Column<bool>(type: "bit", nullable: false),
                    Delivered = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryStatus", x => x.DeliveryID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryStatus");
        }
    }
}
