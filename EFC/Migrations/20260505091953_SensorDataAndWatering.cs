using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EFC.Migrations
{
    /// <inheritdoc />
    public partial class SensorDataAndWatering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirHumidity");

            migrationBuilder.DropTable(
                name: "LightIntensities");

            migrationBuilder.DropTable(
                name: "SoilHumidity");

            migrationBuilder.DropTable(
                name: "Temperatures");

            migrationBuilder.DropTable(
                name: "WaterIntakes");

            migrationBuilder.CreateTable(
                name: "SensorDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Temperature = table.Column<double>(type: "double precision", nullable: false),
                    AirHumidity = table.Column<double>(type: "double precision", nullable: false),
                    SoilHumidity = table.Column<double>(type: "double precision", nullable: false),
                    LightIntensity = table.Column<double>(type: "double precision", nullable: false),
                    PlantMAC = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorDatas_Plants_PlantMAC",
                        column: x => x.PlantMAC,
                        principalTable: "Plants",
                        principalColumn: "MAC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Waterings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PumpTime = table.Column<double>(type: "double precision", nullable: false),
                    WaterTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PredictedWaterTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WaterLevel = table.Column<double>(type: "double precision", nullable: false),
                    PlantMAC = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waterings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Waterings_Plants_PlantMAC",
                        column: x => x.PlantMAC,
                        principalTable: "Plants",
                        principalColumn: "MAC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensorDatas_PlantMAC",
                table: "SensorDatas",
                column: "PlantMAC");

            migrationBuilder.CreateIndex(
                name: "IX_Waterings_PlantMAC",
                table: "Waterings",
                column: "PlantMAC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorDatas");

            migrationBuilder.DropTable(
                name: "Waterings");

            migrationBuilder.CreateTable(
                name: "AirHumidity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlantMAC = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirHumidity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirHumidity_Plants_PlantMAC",
                        column: x => x.PlantMAC,
                        principalTable: "Plants",
                        principalColumn: "MAC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LightIntensities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlantMAC = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightIntensities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LightIntensities_Plants_PlantMAC",
                        column: x => x.PlantMAC,
                        principalTable: "Plants",
                        principalColumn: "MAC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoilHumidity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlantMAC = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoilHumidity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoilHumidity_Plants_PlantMAC",
                        column: x => x.PlantMAC,
                        principalTable: "Plants",
                        principalColumn: "MAC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Temperatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlantMAC = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temperatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Temperatures_Plants_PlantMAC",
                        column: x => x.PlantMAC,
                        principalTable: "Plants",
                        principalColumn: "MAC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaterIntakes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlantMAC = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterIntakes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterIntakes_Plants_PlantMAC",
                        column: x => x.PlantMAC,
                        principalTable: "Plants",
                        principalColumn: "MAC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirHumidity_PlantMAC",
                table: "AirHumidity",
                column: "PlantMAC");

            migrationBuilder.CreateIndex(
                name: "IX_LightIntensities_PlantMAC",
                table: "LightIntensities",
                column: "PlantMAC");

            migrationBuilder.CreateIndex(
                name: "IX_SoilHumidity_PlantMAC",
                table: "SoilHumidity",
                column: "PlantMAC");

            migrationBuilder.CreateIndex(
                name: "IX_Temperatures_PlantMAC",
                table: "Temperatures",
                column: "PlantMAC");

            migrationBuilder.CreateIndex(
                name: "IX_WaterIntakes_PlantMAC",
                table: "WaterIntakes",
                column: "PlantMAC");
        }
    }
}
