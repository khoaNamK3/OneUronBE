using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneUron.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProcessAndSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Processes_ProcessId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_ProcessId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "ProcessId",
                table: "Subjects");

            migrationBuilder.CreateTable(
                name: "ProcessSubject",
                columns: table => new
                {
                    ProcessesId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessSubject", x => new { x.ProcessesId, x.SubjectsId });
                    table.ForeignKey(
                        name: "FK_ProcessSubject_Processes_ProcessesId",
                        column: x => x.ProcessesId,
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessSubject_Subjects_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessSubject_SubjectsId",
                table: "ProcessSubject",
                column: "SubjectsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessSubject");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcessId",
                table: "Subjects",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_ProcessId",
                table: "Subjects",
                column: "ProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Processes_ProcessId",
                table: "Subjects",
                column: "ProcessId",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
