using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedRoute.Migrations
{
    public partial class AddInsuranceCodeAndDeleteServeStatusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_ServeStatuses_ServeStatusId",
                table: "MedicalRecords");

            migrationBuilder.DropTable(
                name: "ServeStatuses");

            migrationBuilder.DropIndex(
                name: "IX_MedicalRecords_ServeStatusId",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Job",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ServeStatusId",
                table: "MedicalRecords");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Gender",
                table: "Users",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CitizenIdentificationCardNumber",
                table: "Users",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceCode",
                table: "Users",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceCode",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Gender",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CitizenIdentificationCardNumber",
                table: "Users",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ServeStatusId",
                table: "MedicalRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ServeStatuses",
                columns: table => new
                {
                    ServeStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServeStatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServeStatuses", x => x.ServeStatusId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_ServeStatusId",
                table: "MedicalRecords",
                column: "ServeStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_ServeStatuses_ServeStatusId",
                table: "MedicalRecords",
                column: "ServeStatusId",
                principalTable: "ServeStatuses",
                principalColumn: "ServeStatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
