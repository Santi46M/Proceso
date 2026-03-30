using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class InitialMetrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "metrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    host = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "local"),
                    cpu_percent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    mem_total_mb = table.Column<int>(type: "integer", nullable: false),
                    mem_used_mb = table.Column<int>(type: "integer", nullable: false),
                    mem_percent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    disk_total_gb = table.Column<int>(type: "integer", nullable: false),
                    disk_used_gb = table.Column<int>(type: "integer", nullable: false),
                    disk_percent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metrics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_metrics_created_at",
                table: "metrics",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "idx_metrics_host_created_at",
                table: "metrics",
                columns: new[] { "host", "created_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "metrics");
        }
    }
}
