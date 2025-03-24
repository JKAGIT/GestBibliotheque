using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestBibliotheque.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTableReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateRetourEstimee = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Annuler = table.Column<bool>(type: "bit", nullable: false),
                    IDUsager = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDLivre = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reservations_Livres_IDLivre",
                        column: x => x.IDLivre,
                        principalTable: "Livres",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Usagers_IDUsager",
                        column: x => x.IDUsager,
                        principalTable: "Usagers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emprunts_IDReservation",
                table: "Emprunts",
                column: "IDReservation",
                unique: true,
                filter: "[IDReservation] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_IDLivre",
                table: "Reservations",
                column: "IDLivre");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_IDUsager",
                table: "Reservations",
                column: "IDUsager");

            migrationBuilder.AddForeignKey(
                name: "FK_Emprunts_Reservations_IDReservation",
                table: "Emprunts",
                column: "IDReservation",
                principalTable: "Reservations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emprunts_Reservations_IDReservation",
                table: "Emprunts");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Emprunts_IDReservation",
                table: "Emprunts");
        }
    }
}
