using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la vIlla", new DateTime(2025, 1, 31, 13, 24, 39, 684, DateTimeKind.Local).AddTicks(3795), new DateTime(2025, 1, 31, 13, 24, 39, 684, DateTimeKind.Local).AddTicks(3748), "", 50, "Villa Real", 5, 200.0 },
                    { 2, "", "Detalle de la vIlla", new DateTime(2025, 1, 31, 13, 24, 39, 684, DateTimeKind.Local).AddTicks(3800), new DateTime(2025, 1, 31, 13, 24, 39, 684, DateTimeKind.Local).AddTicks(3799), "", 40, "Premium Vista a la Piscina", 4, 150.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
