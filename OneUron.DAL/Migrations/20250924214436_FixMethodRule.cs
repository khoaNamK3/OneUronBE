using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneUron.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixMethodRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Effectiveness",
                table: "MethodRuleConditions",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "MethodRuleConditions",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Effectiveness",
                table: "MethodRuleConditions");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "MethodRuleConditions");
        }
    }
}
