using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Jueves.Migrations
{
    /// <inheritdoc />
    public partial class AddRestauranteEstadoYConfiguracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConfiguracionCompletada",
                table: "Restaurantes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Restaurantes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfiguracionCompletada",
                table: "Restaurantes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Restaurantes");
        }
    }
}
