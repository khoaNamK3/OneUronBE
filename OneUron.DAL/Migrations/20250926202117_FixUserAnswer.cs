using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneUron.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixUserAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAnswers",
                table: "UserAnswers");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserAnswers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAnswers",
                table: "UserAnswers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UserId_EvaluationQuestionId",
                table: "UserAnswers",
                columns: new[] { "UserId", "EvaluationQuestionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAnswers",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_UserId_EvaluationQuestionId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserAnswers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAnswers",
                table: "UserAnswers",
                columns: new[] { "UserId", "EvaluationQuestionId", "ChoiceId" });
        }
    }
}
