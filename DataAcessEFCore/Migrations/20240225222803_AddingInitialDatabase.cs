using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcessEFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddingInitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUrls_Urls_UrlId",
                table: "UserUrls");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUrls_Users_UserId",
                table: "UserUrls");

            migrationBuilder.DropIndex(
                name: "IX_UserUrls_UrlId",
                table: "UserUrls");

            migrationBuilder.DropIndex(
                name: "IX_UserCredentials_UserId",
                table: "UserCredentials");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserUrls",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UrlId",
                table: "UserUrls",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserUrls",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserCredentials",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: false),
                    RefreshTokenExpired = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserUrls_UrlId",
                table: "UserUrls",
                column: "UrlId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCredentials_UserId",
                table: "UserCredentials",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUrls_Urls_UrlId",
                table: "UserUrls",
                column: "UrlId",
                principalTable: "Urls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUrls_Users_UserId",
                table: "UserUrls",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUrls_Urls_UrlId",
                table: "UserUrls");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUrls_Users_UserId",
                table: "UserUrls");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropIndex(
                name: "IX_UserUrls_UrlId",
                table: "UserUrls");

            migrationBuilder.DropIndex(
                name: "IX_UserCredentials_UserId",
                table: "UserCredentials");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserUrls",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "UrlId",
                table: "UserUrls",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "UserUrls",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "UserCredentials",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_UserUrls_UrlId",
                table: "UserUrls",
                column: "UrlId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCredentials_UserId",
                table: "UserCredentials",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUrls_Urls_UrlId",
                table: "UserUrls",
                column: "UrlId",
                principalTable: "Urls",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserUrls_Users_UserId",
                table: "UserUrls",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
