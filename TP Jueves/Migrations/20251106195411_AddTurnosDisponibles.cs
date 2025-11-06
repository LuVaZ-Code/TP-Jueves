using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Jueves.Migrations
{
    /// <inheritdoc />
    public partial class AddTurnosDisponibles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Mesas_MesaId",
                table: "Reservas");

            migrationBuilder.AlterColumn<int>(
                name: "MesaId",
                table: "Reservas",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "TurnoDisponibleId",
                table: "Reservas",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TurnosDisponibles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RestauranteId = table.Column<int>(type: "INTEGER", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Horario = table.Column<int>(type: "INTEGER", nullable: false),
                    CapacidadMaxima = table.Column<int>(type: "INTEGER", nullable: false),
                    CapacidadUsada = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurnosDisponibles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurnosDisponibles_Restaurantes_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_TurnoDisponibleId",
                table: "Reservas",
                column: "TurnoDisponibleId");

            migrationBuilder.CreateIndex(
                name: "IX_TurnosDisponibles_RestauranteId",
                table: "TurnosDisponibles",
                column: "RestauranteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Mesas_MesaId",
                table: "Reservas",
                column: "MesaId",
                principalTable: "Mesas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_TurnosDisponibles_TurnoDisponibleId",
                table: "Reservas",
                column: "TurnoDisponibleId",
                principalTable: "TurnosDisponibles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Mesas_MesaId",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_TurnosDisponibles_TurnoDisponibleId",
                table: "Reservas");

            migrationBuilder.DropTable(
                name: "TurnosDisponibles");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_TurnoDisponibleId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "TurnoDisponibleId",
                table: "Reservas");

            migrationBuilder.AlterColumn<int>(
                name: "MesaId",
                table: "Reservas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Mesas_MesaId",
                table: "Reservas",
                column: "MesaId",
                principalTable: "Mesas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
