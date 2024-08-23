using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalWebApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessageDefaultDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "Messages",
                type: "datetime",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
