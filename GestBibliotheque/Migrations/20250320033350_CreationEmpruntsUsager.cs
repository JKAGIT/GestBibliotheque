using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestBibliotheque.Migrations
{
    /// <inheritdoc />
    public partial class CreationEmpruntsUsager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Telephone",
                table: "Utilisateurs",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Usagers",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prenoms = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Courriel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usagers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Emprunts",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDReservation = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateRetourPrevue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IDUsager = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDLivre = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprunts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Emprunts_Livres_IDLivre",
                        column: x => x.IDLivre,
                        principalTable: "Livres",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Emprunts_Usagers_IDUsager",
                        column: x => x.IDUsager,
                        principalTable: "Usagers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emprunts_IDLivre",
                table: "Emprunts",
                column: "IDLivre");

            migrationBuilder.CreateIndex(
                name: "IX_Emprunts_IDUsager",
                table: "Emprunts",
                column: "IDUsager");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emprunts");

            migrationBuilder.DropTable(
                name: "Usagers");

            migrationBuilder.AlterColumn<string>(
                name: "Telephone",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(14)",
                oldMaxLength: 14);
        }
    }
}
