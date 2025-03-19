using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestBibliotheque.Migrations
{
    /// <inheritdoc />
    public partial class CreationInitiale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Livres",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Auteur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    IDCategorie = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livres", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Livres_Categories_IDCategorie",
                        column: x => x.IDCategorie,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Livres_IDCategorie",
                table: "Livres",
                column: "IDCategorie");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Livres");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
