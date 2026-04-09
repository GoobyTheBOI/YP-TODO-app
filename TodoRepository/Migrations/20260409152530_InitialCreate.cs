using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TodoRepositories.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    isCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "CreatedDate", "Title", "isCompleted" },
                values: new object[,]
                {
                    { new Guid("20b3ff4d-ee8e-414d-823f-65ae3600e2af"), new DateTime(2026, 4, 9, 15, 25, 27, 39, DateTimeKind.Utc).AddTicks(737), "First dummy todo", false },
                    { new Guid("58e50c68-460e-40f2-a2df-792b1cf17d4c"), new DateTime(2026, 4, 9, 15, 25, 27, 39, DateTimeKind.Utc).AddTicks(1122), "Second dummy todo", false },
                    { new Guid("5d5794cc-8d91-4a32-bc8c-2f9b72b9faa3"), new DateTime(2026, 4, 9, 15, 25, 27, 39, DateTimeKind.Utc).AddTicks(1130), "Third dummy todo", true },
                    { new Guid("ada242e8-f933-421e-bcca-49fdcff41096"), new DateTime(2026, 4, 9, 15, 25, 27, 39, DateTimeKind.Utc).AddTicks(1134), "Fourth dummy todo", false },
                    { new Guid("ddb976e7-9344-4533-95de-ebcb71bbd969"), new DateTime(2026, 4, 9, 15, 25, 27, 39, DateTimeKind.Utc).AddTicks(1138), "Fifth dummy todo", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}
