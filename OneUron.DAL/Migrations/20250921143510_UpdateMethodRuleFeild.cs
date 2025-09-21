using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneUron.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMethodRuleFeild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Effectiveness",
                table: "MethodRules");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "MethodRules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Effectiveness",
                table: "MethodRules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "MethodRules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
