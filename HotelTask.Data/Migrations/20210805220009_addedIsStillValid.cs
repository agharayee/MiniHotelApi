using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelTask.Data.Migrations
{
    public partial class addedIsStillValid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBookingActive",
                table: "RoomBookings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBookingActive",
                table: "RoomBookings");
        }
    }
}
