using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DalilAcademy.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailInstructor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Instructors",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Instructors",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_Email",
                table: "Instructors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_UserId",
                table: "Instructors",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_Users_UserId",
                table: "Instructors",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_Users_UserId",
                table: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_Email",
                table: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_UserId",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Instructors");
        }
    }
}
