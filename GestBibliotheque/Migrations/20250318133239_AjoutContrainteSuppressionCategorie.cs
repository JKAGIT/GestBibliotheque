using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestBibliotheque.Migrations
{
    /// <inheritdoc />
    public partial class AjoutContrainteSuppressionCategorie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livres_Categories_IDCategorie",
                table: "Livres");

            migrationBuilder.AddForeignKey(
                name: "FK_Livres_Categories_IDCategorie",
                table: "Livres",
                column: "IDCategorie",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livres_Categories_IDCategorie",
                table: "Livres");

            migrationBuilder.AddForeignKey(
                name: "FK_Livres_Categories_IDCategorie",
                table: "Livres",
                column: "IDCategorie",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
