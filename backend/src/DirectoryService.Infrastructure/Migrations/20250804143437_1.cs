using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectoryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_positions_name",
                schema: "department",
                table: "positions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_departments_position",
                schema: "department",
                table: "departments_position");

            migrationBuilder.DropIndex(
                name: "IX_departments_position_department_id",
                schema: "department",
                table: "departments_position");

            migrationBuilder.DropPrimaryKey(
                name: "PK_departments_locations",
                schema: "department",
                table: "departments_locations");

            migrationBuilder.DropIndex(
                name: "IX_departments_locations_department_id",
                schema: "department",
                table: "departments_locations");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                schema: "department",
                table: "departments_position",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                schema: "department",
                table: "departments_locations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_departments_position",
                schema: "department",
                table: "departments_position",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_departments_locations",
                schema: "department",
                table: "departments_locations",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_positions_name",
                schema: "department",
                table: "positions",
                column: "name",
                unique: true,
                filter: "\"is_active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_departments_position_department_id_position_id",
                schema: "department",
                table: "departments_position",
                columns: new[] { "department_id", "position_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_departments_position_position_id",
                schema: "department",
                table: "departments_position",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "IX_departments_locations_department_id_location_id",
                schema: "department",
                table: "departments_locations",
                columns: new[] { "department_id", "location_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_departments_locations_location_id",
                schema: "department",
                table: "departments_locations",
                column: "location_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_positions_name",
                schema: "department",
                table: "positions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_departments_position",
                schema: "department",
                table: "departments_position");

            migrationBuilder.DropIndex(
                name: "IX_departments_position_department_id_position_id",
                schema: "department",
                table: "departments_position");

            migrationBuilder.DropIndex(
                name: "IX_departments_position_position_id",
                schema: "department",
                table: "departments_position");

            migrationBuilder.DropPrimaryKey(
                name: "PK_departments_locations",
                schema: "department",
                table: "departments_locations");

            migrationBuilder.DropIndex(
                name: "IX_departments_locations_department_id_location_id",
                schema: "department",
                table: "departments_locations");

            migrationBuilder.DropIndex(
                name: "IX_departments_locations_location_id",
                schema: "department",
                table: "departments_locations");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "department",
                table: "departments_position");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "department",
                table: "departments_locations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_departments_position",
                schema: "department",
                table: "departments_position",
                columns: new[] { "position_id", "department_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_departments_locations",
                schema: "department",
                table: "departments_locations",
                columns: new[] { "location_id", "department_id" });

            migrationBuilder.CreateIndex(
                name: "IX_positions_name",
                schema: "department",
                table: "positions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_departments_position_department_id",
                schema: "department",
                table: "departments_position",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_departments_locations_department_id",
                schema: "department",
                table: "departments_locations",
                column: "department_id");
        }
    }
}
