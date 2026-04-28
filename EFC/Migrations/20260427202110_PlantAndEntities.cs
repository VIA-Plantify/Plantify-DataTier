using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EFC.Migrations
{
    /// <inheritdoc />
    public partial class PlantAndEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    MAC = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    OptimalTemperature = table.Column<double>(type: "double precision", nullable: false),
                    OptimalAirHumidity = table.Column<double>(type: "double precision", nullable: false),
                    OptimalSoilHumidity = table.Column<double>(type: "double precision", nullable: false),
                    OptimalLightIntensity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.MAC);
                    table.ForeignKey(
                        name: "FK_Plants_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirHumidity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<double>(type: "double precision", nullable: true),
                    PlantMAC = table.Column<string>(type: "text", nullable: false)
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
                    Value = table.Column<double>(type: "double precision", nullable: true),
                    PlantMAC = table.Column<string>(type: "text", nullable: false)
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
                    Value = table.Column<double>(type: "double precision", nullable: true),
                    PlantMAC = table.Column<string>(type: "text", nullable: false)
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
                    Value = table.Column<double>(type: "double precision", nullable: true),
                    PlantMAC = table.Column<string>(type: "text", nullable: false)
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
                name: "IX_Plants_Username",
                table: "Plants",
                column: "Username");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "Plants");
        }
    }
}
