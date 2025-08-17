using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetMessagingApplication.Server.Migrations
{
    /// <inheritdoc />
    public partial class CompositeKeySettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Settings",
                table: "Settings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Settings",
                table: "Settings",
                columns: new[] { "SettingsId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Settings_SettingsId",
                table: "Settings",
                column: "SettingsId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Settings",
                table: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Settings_SettingsId",
                table: "Settings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Settings",
                table: "Settings",
                column: "SettingsId");
        }
    }
}
