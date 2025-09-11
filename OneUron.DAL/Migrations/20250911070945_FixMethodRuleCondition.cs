using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneUron.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixMethodRuleCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MethodRuleConditions_MethodRules_MethodRuleId",
                table: "MethodRuleConditions");

            migrationBuilder.DropIndex(
                name: "IX_MethodRuleConditions_MethodRuleId",
                table: "MethodRuleConditions");

            migrationBuilder.DropColumn(
                name: "MethodRuleId",
                table: "MethodRuleConditions");

            migrationBuilder.AddColumn<Guid>(
                name: "MethodRuleConditionId",
                table: "MethodRules",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MethodRules_MethodRuleConditionId",
                table: "MethodRules",
                column: "MethodRuleConditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MethodRules_MethodRuleConditions_MethodRuleConditionId",
                table: "MethodRules",
                column: "MethodRuleConditionId",
                principalTable: "MethodRuleConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MethodRules_MethodRuleConditions_MethodRuleConditionId",
                table: "MethodRules");

            migrationBuilder.DropIndex(
                name: "IX_MethodRules_MethodRuleConditionId",
                table: "MethodRules");

            migrationBuilder.DropColumn(
                name: "MethodRuleConditionId",
                table: "MethodRules");

            migrationBuilder.AddColumn<Guid>(
                name: "MethodRuleId",
                table: "MethodRuleConditions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MethodRuleConditions_MethodRuleId",
                table: "MethodRuleConditions",
                column: "MethodRuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_MethodRuleConditions_MethodRules_MethodRuleId",
                table: "MethodRuleConditions",
                column: "MethodRuleId",
                principalTable: "MethodRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
