using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "livros",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    titulo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    autor = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    quantidadedisponivel = table.Column<short>(name: "quantidade-disponivel", type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_livros", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "emprestimos",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    livroid = table.Column<int>(name: "livro-id", type: "uniqueidentifier", nullable: false),
                    dataemprestimo = table.Column<DateTime>(name: "data-emprestimo", type: "TEXT", nullable: false),
                    datadevolucao = table.Column<DateTime>(name: "data-devolucao", type: "TEXT", nullable: true),
                    status = table.Column<int>(type: "smallint", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emprestimos", x => x.id);
                    table.ForeignKey(
                        name: "FK_emprestimos_livros_livro-id",
                        column: x => x.livroid,
                        principalTable: "livros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_emprestimos_livro-id",
                table: "emprestimos",
                column: "livro-id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "emprestimos");

            migrationBuilder.DropTable(
                name: "livros");
        }
    }
}
