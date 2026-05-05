using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFC.Migrations
{
    /// <inheritdoc />
    public partial class WateringWithPumpTimeInSeconds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PumpTime",
                table: "Waterings");

            migrationBuilder.RenameColumn(
                name: "WaterTime",
                table: "Waterings",
                newName: "PredictedFutureWaterTime");

            migrationBuilder.RenameColumn(
                name: "PredictedWaterTime",
                table: "Waterings",
                newName: "LastWaterTime");

            migrationBuilder.AddColumn<int>(
                name: "PumpTimeInSeconds",
                table: "Waterings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "SensorDatas",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PumpTimeInSeconds",
                table: "Waterings");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "SensorDatas");

            migrationBuilder.RenameColumn(
                name: "PredictedFutureWaterTime",
                table: "Waterings",
                newName: "WaterTime");

            migrationBuilder.RenameColumn(
                name: "LastWaterTime",
                table: "Waterings",
                newName: "PredictedWaterTime");

            migrationBuilder.AddColumn<double>(
                name: "PumpTime",
                table: "Waterings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
