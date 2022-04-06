using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationAndAuthorization.Migrations
{
    public partial class adddatapropertiestoactivitylog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "action_on",
                table: "audit_logs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 3, 18, 14, 59, 41, 533, DateTimeKind.Local).AddTicks(1866),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 3, 18, 13, 56, 33, 222, DateTimeKind.Local).AddTicks(7020));

            migrationBuilder.AddColumn<string>(
                name: "query_string",
                table: "ActivityLogs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "query_string",
                table: "ActivityLogs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "action_on",
                table: "audit_logs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 3, 18, 13, 56, 33, 222, DateTimeKind.Local).AddTicks(7020),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 3, 18, 14, 59, 41, 533, DateTimeKind.Local).AddTicks(1866));
        }
    }
}
