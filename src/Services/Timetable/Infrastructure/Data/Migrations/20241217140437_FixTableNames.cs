using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Timetables_timetable_id",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Timetables",
                table: "Timetables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "Timetables",
                newName: "timetables");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "appointments");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_timetable_id",
                table: "appointments",
                newName: "IX_appointments_timetable_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_timetables",
                table: "timetables",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_appointments",
                table: "appointments",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_timetables_timetable_id",
                table: "appointments",
                column: "timetable_id",
                principalTable: "timetables",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_timetables_timetable_id",
                table: "appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_timetables",
                table: "timetables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_appointments",
                table: "appointments");

            migrationBuilder.RenameTable(
                name: "timetables",
                newName: "Timetables");

            migrationBuilder.RenameTable(
                name: "appointments",
                newName: "Appointments");

            migrationBuilder.RenameIndex(
                name: "IX_appointments_timetable_id",
                table: "Appointments",
                newName: "IX_Appointments_timetable_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Timetables",
                table: "Timetables",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Timetables_timetable_id",
                table: "Appointments",
                column: "timetable_id",
                principalTable: "Timetables",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
