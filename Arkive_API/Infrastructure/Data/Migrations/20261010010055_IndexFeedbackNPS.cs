using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arkive_API.Migrations
{
    /// <inheritdoc />
    public partial class IndexFeedbackNPS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ARKIVE_NPS_ANIMAL",
                table: "TB_ARKIVE_FEEDBACK_NPS",
                column: "ID_ANIMAL");

            migrationBuilder.CreateIndex(
                name: "IX_ARKIVE_NPS_CLINICA",
                table: "TB_ARKIVE_FEEDBACK_NPS",
                column: "ID_CLINICA");

            migrationBuilder.CreateIndex(
                name: "IX_ARKIVE_NPS_NOTA_DATA",
                table: "TB_ARKIVE_FEEDBACK_NPS",
                columns: new[] { "NR_NOTA", "DT_FEEDBACK" });

            migrationBuilder.CreateIndex(
                name: "IX_ARKIVE_NPS_RESPONSAVEL",
                table: "TB_ARKIVE_FEEDBACK_NPS",
                column: "ID_RESPONSAVEL");

            migrationBuilder.CreateIndex(
                name: "IX_ARKIVE_NPS_VETERINARIO",
                table: "TB_ARKIVE_FEEDBACK_NPS",
                column: "ID_VETERINARIO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ARKIVE_NPS_ANIMAL",
                table: "TB_ARKIVE_FEEDBACK_NPS");

            migrationBuilder.DropIndex(
                name: "IX_ARKIVE_NPS_CLINICA",
                table: "TB_ARKIVE_FEEDBACK_NPS");

            migrationBuilder.DropIndex(
                name: "IX_ARKIVE_NPS_NOTA_DATA",
                table: "TB_ARKIVE_FEEDBACK_NPS");

            migrationBuilder.DropIndex(
                name: "IX_ARKIVE_NPS_RESPONSAVEL",
                table: "TB_ARKIVE_FEEDBACK_NPS");

            migrationBuilder.DropIndex(
                name: "IX_ARKIVE_NPS_VETERINARIO",
                table: "TB_ARKIVE_FEEDBACK_NPS");
        }
    }
}
