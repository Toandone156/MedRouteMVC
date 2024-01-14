using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedRoute.Migrations
{
    public partial class ChangeRelationOfMedicalRecordAndBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MedicalRecords_BookingId",
                table: "MedicalRecords");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Bookings",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "BookingOrder",
                table: "Bookings",
                newName: "Order");

            migrationBuilder.AddColumn<int>(
                name: "BookingOrder",
                table: "MedicalRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MedicalRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_BookingId",
                table: "MedicalRecords",
                column: "BookingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MedicalRecords_BookingId",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "BookingOrder",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MedicalRecords");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "Bookings",
                newName: "BookingOrder");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Bookings",
                newName: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_BookingId",
                table: "MedicalRecords",
                column: "BookingId",
                unique: true,
                filter: "[BookingId] IS NOT NULL");
        }
    }
}
