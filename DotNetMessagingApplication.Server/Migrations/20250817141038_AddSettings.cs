using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetMessagingApplication.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Settings_SettingsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SettingsId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "SettingsId",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Settings_Users_SettingsId",
                table: "Settings",
                column: "SettingsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settings_Users_SettingsId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Settings");

            migrationBuilder.AlterColumn<int>(
                name: "SettingsId",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SettingsId",
                table: "Users",
                column: "SettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Settings_SettingsId",
                table: "Users",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "SettingsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
