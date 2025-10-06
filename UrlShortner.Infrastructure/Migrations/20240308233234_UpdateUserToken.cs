using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcessEFCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "UserTokens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "UserTokens",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "UserTokens");
        }
    }
}
