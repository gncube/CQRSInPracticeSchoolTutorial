using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Course",
                columns: new[] { "CourseID", "Name" },
                values: new object[,]
                {
                    { 1L, "Introduction to Computer Science" },
                    { 2L, "Data Structures and Algorithms" },
                    { 3L, "Operating Systems" },
                    { 4L, "Web Development" },
                    { 5L, "Machine Learning" }
                });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "StudentID", "Email", "Name" },
                values: new object[,]
                {
                    { 1L, "john.doe@email.com", "John Doe" },
                    { 2L, "jane.smith@email.com", "Jane Smith" },
                    { 3L, "emily.jones@email.com", "Emily Jones" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Course",
                keyColumn: "CourseID",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Course",
                keyColumn: "CourseID",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Course",
                keyColumn: "CourseID",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Course",
                keyColumn: "CourseID",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Course",
                keyColumn: "CourseID",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentID",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentID",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "StudentID",
                keyValue: 3L);
        }
    }
}
