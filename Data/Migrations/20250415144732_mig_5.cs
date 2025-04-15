using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "AccountTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Channel",
                table: "AccountTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AccountTransactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "AccountTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "AccountTransactions");

            migrationBuilder.DropColumn(
                name: "Channel",
                table: "AccountTransactions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AccountTransactions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AccountTransactions");
        }
    }
}
