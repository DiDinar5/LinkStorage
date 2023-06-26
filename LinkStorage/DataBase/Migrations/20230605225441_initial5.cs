using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LinkStorage.Migrations
{
    /// <inheritdoc />
    public partial class initial5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SmartContracts",
                table: "SmartContracts");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "SmartContracts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmartContracts",
                table: "SmartContracts",
                column: "Id");
        }
    }
}
