using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryBookingCllient.Migrations
{
    public partial class thir : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExecutiveID",
                table: "RequestRejected",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RequestAccepted",
                columns: table => new
                {
                    RejectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    RequestID = table.Column<int>(type: "int", nullable: false),
                    ExecutiveID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestAccepted", x => x.RejectID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestAccepted");

            migrationBuilder.DropColumn(
                name: "ExecutiveID",
                table: "RequestRejected");
        }
    }
}
