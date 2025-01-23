using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoLambdaFunction.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueEmailConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_Email",
                table: "SurveyResponses",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_Email",
                table: "SurveyResponses");
        }
    }
}
