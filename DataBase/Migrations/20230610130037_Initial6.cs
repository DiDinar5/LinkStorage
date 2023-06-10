using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LinkStorage.Migrations
{
    /// <inheritdoc />
    public partial class Initial6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SmartContracts",
                table: "SmartContracts");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SmartContracts",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmartContracts",
                table: "SmartContracts",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SmartContracts",
                table: "SmartContracts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SmartContracts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmartContracts",
                table: "SmartContracts",
                column: "LinkToContract");
        }
    }
}
