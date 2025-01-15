using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherForGardeners.Migrations
{
    /// <inheritdoc />
    public partial class RenameDayRecommendations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HtmlContent",
                table: "DayRecommendations",
                newName: "Content");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "DayRecommendations",
                newName: "HtmlContent");
        }
    }
}
