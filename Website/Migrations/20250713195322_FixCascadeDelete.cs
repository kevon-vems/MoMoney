using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retire.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentRollovers_Investments_DestinationInvestmentId",
                table: "InvestmentRollovers");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentRollovers_Investments_SourceInvestmentId",
                table: "InvestmentRollovers");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentRollovers_Investments_DestinationInvestmentId",
                table: "InvestmentRollovers",
                column: "DestinationInvestmentId",
                principalTable: "Investments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentRollovers_Investments_SourceInvestmentId",
                table: "InvestmentRollovers",
                column: "SourceInvestmentId",
                principalTable: "Investments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentRollovers_Investments_DestinationInvestmentId",
                table: "InvestmentRollovers");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentRollovers_Investments_SourceInvestmentId",
                table: "InvestmentRollovers");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentRollovers_Investments_DestinationInvestmentId",
                table: "InvestmentRollovers",
                column: "DestinationInvestmentId",
                principalTable: "Investments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentRollovers_Investments_SourceInvestmentId",
                table: "InvestmentRollovers",
                column: "SourceInvestmentId",
                principalTable: "Investments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
