using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetMessagingApplication.Server.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendship");

            migrationBuilder.AddColumn<int>(
                name: "SettingsId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Relationship",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    OtherPersonId = table.Column<int>(type: "INTEGER", nullable: false),
                    RelationshipType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationship", x => new { x.UserId, x.OtherPersonId });
                    table.ForeignKey(
                        name: "FK_Relationship_Users_OtherPersonId",
                        column: x => x.OtherPersonId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Relationship_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimeLimit = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    SendEmailNotifications = table.Column<bool>(type: "INTEGER", nullable: false),
                    PrivacyLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingsId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SettingsId",
                table: "Users",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationship_OtherPersonId",
                table: "Relationship",
                column: "OtherPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Settings_SettingsId",
                table: "Users",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "SettingsId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Settings_SettingsId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Relationship");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Users_SettingsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Friendship",
                columns: table => new
                {
                    FollowsId = table.Column<int>(type: "INTEGER", nullable: false),
                    FollowedById = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendship", x => new { x.FollowsId, x.FollowedById });
                    table.ForeignKey(
                        name: "FK_Friendship_Users_FollowedById",
                        column: x => x.FollowedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendship_Users_FollowsId",
                        column: x => x.FollowsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_FollowedById",
                table: "Friendship",
                column: "FollowedById");
        }
    }
}
