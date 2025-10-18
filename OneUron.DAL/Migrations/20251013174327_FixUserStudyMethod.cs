using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneUron.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixUserStudyMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudyMethods_UserId",
                table: "StudyMethods");

            migrationBuilder.CreateIndex(
                name: "IX_StudyMethods_UserId",
                table: "StudyMethods",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudyMethods_UserId",
                table: "StudyMethods");

            migrationBuilder.CreateIndex(
                name: "IX_StudyMethods_UserId",
                table: "StudyMethods",
                column: "UserId");
        }
    }
}
