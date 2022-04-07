using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationAndAuthorization.Migrations
{
    public partial class emailTemplateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityLogs",
                table: "ActivityLogs");

            migrationBuilder.RenameTable(
                name: "ActivityLogs",
                newName: "activitylogs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "action_on",
                table: "audit_logs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 4, 2, 19, 49, 27, 273, DateTimeKind.Local).AddTicks(9377),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 3, 18, 14, 59, 41, 533, DateTimeKind.Local).AddTicks(1866));

            migrationBuilder.AddPrimaryKey(
                name: "PK_activitylogs",
                table: "activitylogs",
                column: "id");

            migrationBuilder.CreateTable(
                name: "template",
                columns: table => new
                {
                    template_id = table.Column<long>(type: "bigint", maxLength: 20, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    template = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_template", x => x.template_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "template");

            migrationBuilder.DropPrimaryKey(
                name: "PK_activitylogs",
                table: "activitylogs");

            migrationBuilder.RenameTable(
                name: "activitylogs",
                newName: "ActivityLogs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "action_on",
                table: "audit_logs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 3, 18, 14, 59, 41, 533, DateTimeKind.Local).AddTicks(1866),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 4, 2, 19, 49, 27, 273, DateTimeKind.Local).AddTicks(9377));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityLogs",
                table: "ActivityLogs",
                column: "id");
        }
    }
}
