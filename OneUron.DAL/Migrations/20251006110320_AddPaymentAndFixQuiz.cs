using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneUron.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentAndFixQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuizAttempts_Users_UserId",
                table: "UserQuizAttempts");

            migrationBuilder.DropTable(
                name: "QuizHistories");

            migrationBuilder.DropTable(
                name: "QuizUser");

            migrationBuilder.DropIndex(
                name: "IX_UserQuizAttempts_UserId",
                table: "UserQuizAttempts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserQuizAttempts");

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Tokens");

            migrationBuilder.RenameColumn(
                name: "RefeshToken",
                table: "Tokens",
                newName: "RefreshToken");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Quizzes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserQuizAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionChoiceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_QuestionChoices_QuestionChoiceId",
                        column: x => x.QuestionChoiceId,
                        principalTable: "QuestionChoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_UserQuizAttempts_UserQuizAttemptId",
                        column: x => x.UserQuizAttemptId,
                        principalTable: "UserQuizAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberShipPlanId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_MemberShipPlans_MemberShipPlanId",
                        column: x => x.MemberShipPlanId,
                        principalTable: "MemberShipPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payment_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_UserId",
                table: "Quizzes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionChoiceId",
                table: "Answers",
                column: "QuestionChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_UserQuizAttemptId",
                table: "Answers",
                column: "UserQuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_MemberShipPlanId",
                table: "Payment",
                column: "MemberShipPlanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserId",
                table: "Payment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Users_UserId",
                table: "Quizzes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Users_UserId",
                table: "Quizzes");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_UserId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Quizzes");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "Tokens",
                newName: "RefeshToken");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserQuizAttempts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Tokens",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "QuizHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserQuizAttemptId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizHistories_QuestionChoices_ChoiceId",
                        column: x => x.ChoiceId,
                        principalTable: "QuestionChoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuizHistories_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuizHistories_UserQuizAttempts_UserQuizAttemptId",
                        column: x => x.UserQuizAttemptId,
                        principalTable: "UserQuizAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuizUser",
                columns: table => new
                {
                    QuizzesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizUser", x => new { x.QuizzesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_QuizUser_Quizzes_QuizzesId",
                        column: x => x.QuizzesId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAttempts_UserId",
                table: "UserQuizAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizHistories_ChoiceId",
                table: "QuizHistories",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizHistories_QuestionId",
                table: "QuizHistories",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizHistories_UserQuizAttemptId",
                table: "QuizHistories",
                column: "UserQuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizUser_UsersId",
                table: "QuizUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuizAttempts_Users_UserId",
                table: "UserQuizAttempts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
