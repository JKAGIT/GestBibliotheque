using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestBibliotheque.Migrations
{
    /// <inheritdoc />
    public partial class AjouterTableRetour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Retours",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDEmprunt = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateRetour = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retours", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Retours_Emprunts_IDEmprunt",
                        column: x => x.IDEmprunt,
                        principalTable: "Emprunts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Retours_IDEmprunt",
                table: "Retours",
                column: "IDEmprunt",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Retours");
        }
    }
}
