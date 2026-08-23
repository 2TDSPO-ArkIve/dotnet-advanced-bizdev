using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arkive_API.Migrations
{
    /// <inheritdoc />
    public partial class InicialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_ARKIVE_ANIMAL",
                columns: table => new
                {
                    ID_ANIMAL = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ARKIVE_ANIMAL", x => x.ID_ANIMAL);
                });

            migrationBuilder.CreateTable(
                name: "TB_ARKIVE_CATEGORIA_DOENCA",
                columns: table => new
                {
                    ID_CATEGORIA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_CATEGORIA = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ST_ATIVO = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ARKIVE_CATEGORIA_DOENCA", x => x.ID_CATEGORIA);
                });

            migrationBuilder.CreateTable(
                name: "TB_ARKIVE_CLINICA",
                columns: table => new
                {
                    ID_CLINICA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ARKIVE_CLINICA", x => x.ID_CLINICA);
                });

            migrationBuilder.CreateTable(
                name: "TB_ARKIVE_CONSULTA",
                columns: table => new
                {
                    ID_CONSULTA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ARKIVE_CONSULTA", x => x.ID_CONSULTA);
                });

            migrationBuilder.CreateTable(
                name: "TB_ARKIVE_ESPECIE",
                columns: table => new
                {
                    ID_ESPECIE = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_ESPECIE = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    ST_ATIVO = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ARKIVE_ESPECIE", x => x.ID_ESPECIE);
                });

            migrationBuilder.CreateTable(
                name: "TB_ARKIVE_FEEDBACK_NPS",
                columns: table => new
                {
                    ID_FEEDBACK_NPS = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_RESPONSAVEL = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ID_ANIMAL = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ID_CLINICA = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ID_CONSULTA = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ID_VETERINARIO = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NR_NOTA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DS_COMENTARIO = table.Column<string>(type: "CLOB", nullable: true),
                    DT_FEEDBACK = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ARKIVE_FEEDBACK_NPS", x => x.ID_FEEDBACK_NPS);
                });

            migrationBuilder.CreateTable(
                name: "TB_ARKIVE_RESPONSAVEL",
                columns: table => new
                {
                    ID_RESPONSAVEL = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ARKIVE_RESPONSAVEL", x => x.ID_RESPONSAVEL);
                });

            migrationBuilder.CreateTable(
                name: "TB_ARKIVE_VETERINARIO",
                columns: table => new
                {
                    ID_VETERINARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ARKIVE_VETERINARIO", x => x.ID_VETERINARIO);
                });

            migrationBuilder.CreateTable(
                name: "TB_ARKIVE_DOENCA",
                columns: table => new
                {
                    ID_DOENCA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_DOENCA = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ID_CATEGORIA = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DS_DOENCA = table.Column<string>(type: "CLOB", nullable: true),
                    CD_CID_VET = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    DS_SINTOMAS = table.Column<string>(type: "CLOB", nullable: true),
                    ST_ATIVO = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ARKIVE_DOENCA", x => x.ID_DOENCA);
                    table.ForeignKey(
                        name: "FK_TB_ARKIVE_DOENCA_TB_ARKIVE_CATEGORIA_DOENCA_ID_CATEGORIA",
                        column: x => x.ID_CATEGORIA,
                        principalTable: "TB_ARKIVE_CATEGORIA_DOENCA",
                        principalColumn: "ID_CATEGORIA");
                });

            migrationBuilder.CreateTable(
                name: "TB_ARKIVE_RACA",
                columns: table => new
                {
                    ID_RACA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NM_RACA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    ID_ESPECIE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TP_PORTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ST_ATIVO = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ARKIVE_RACA", x => x.ID_RACA);
                    table.ForeignKey(
                        name: "FK_TB_ARKIVE_RACA_TB_ARKIVE_ESPECIE_ID_ESPECIE",
                        column: x => x.ID_ESPECIE,
                        principalTable: "TB_ARKIVE_ESPECIE",
                        principalColumn: "ID_ESPECIE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_ARKIVE_PREDISPOSICAO",
                columns: table => new
                {
                    ID_PREDISPOSICAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_ESPECIE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID_RACA = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ID_DOENCA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ARKIVE_PREDISPOSICAO", x => x.ID_PREDISPOSICAO);
                    table.ForeignKey(
                        name: "FK_TB_ARKIVE_PREDISPOSICAO_TB_ARKIVE_DOENCA_ID_DOENCA",
                        column: x => x.ID_DOENCA,
                        principalTable: "TB_ARKIVE_DOENCA",
                        principalColumn: "ID_DOENCA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_ARKIVE_PREDISPOSICAO_TB_ARKIVE_ESPECIE_ID_ESPECIE",
                        column: x => x.ID_ESPECIE,
                        principalTable: "TB_ARKIVE_ESPECIE",
                        principalColumn: "ID_ESPECIE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_ARKIVE_PREDISPOSICAO_TB_ARKIVE_RACA_ID_RACA",
                        column: x => x.ID_RACA,
                        principalTable: "TB_ARKIVE_RACA",
                        principalColumn: "ID_RACA");
                });

            migrationBuilder.CreateIndex(
                name: "UQ_ARKIVE_CATEGORIA_NOME",
                table: "TB_ARKIVE_CATEGORIA_DOENCA",
                column: "NM_CATEGORIA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_ARKIVE_DOENCA_ID_CATEGORIA",
                table: "TB_ARKIVE_DOENCA",
                column: "ID_CATEGORIA");

            migrationBuilder.CreateIndex(
                name: "UQ_ARKIVE_DOENCA_NOME",
                table: "TB_ARKIVE_DOENCA",
                column: "NM_DOENCA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_ARKIVE_ESPECIE_NOME",
                table: "TB_ARKIVE_ESPECIE",
                column: "NM_ESPECIE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_ARKIVE_PREDISPOSICAO_ID_DOENCA",
                table: "TB_ARKIVE_PREDISPOSICAO",
                column: "ID_DOENCA");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ARKIVE_PREDISPOSICAO_ID_RACA",
                table: "TB_ARKIVE_PREDISPOSICAO",
                column: "ID_RACA");

            migrationBuilder.CreateIndex(
                name: "UX_ARKIVE_PREDISPOSICAO",
                table: "TB_ARKIVE_PREDISPOSICAO",
                columns: new[] { "ID_ESPECIE", "ID_RACA", "ID_DOENCA" },
                unique: true,
                filter: "\"ID_RACA\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_ARKIVE_RACA_ESPECIE_NOME",
                table: "TB_ARKIVE_RACA",
                columns: new[] { "ID_ESPECIE", "NM_RACA" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_ARKIVE_ANIMAL");

            migrationBuilder.DropTable(
                name: "TB_ARKIVE_CLINICA");

            migrationBuilder.DropTable(
                name: "TB_ARKIVE_CONSULTA");

            migrationBuilder.DropTable(
                name: "TB_ARKIVE_FEEDBACK_NPS");

            migrationBuilder.DropTable(
                name: "TB_ARKIVE_PREDISPOSICAO");

            migrationBuilder.DropTable(
                name: "TB_ARKIVE_RESPONSAVEL");

            migrationBuilder.DropTable(
                name: "TB_ARKIVE_VETERINARIO");

            migrationBuilder.DropTable(
                name: "TB_ARKIVE_DOENCA");

            migrationBuilder.DropTable(
                name: "TB_ARKIVE_RACA");

            migrationBuilder.DropTable(
                name: "TB_ARKIVE_CATEGORIA_DOENCA");

            migrationBuilder.DropTable(
                name: "TB_ARKIVE_ESPECIE");
        }
    }
}
