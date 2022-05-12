using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList.Infrastructure.Migrations
{
    public partial class ProperlySetTaskListIdForTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Tasks",
                newName: "nvarchar(80)");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Tasks",
                newName: "nvarchar(max)");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TaskLists",
                newName: "nvarchar(80)");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TaskLists",
                newName: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nvarchar(max)",
                table: "Tasks",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "nvarchar(80)",
                table: "Tasks",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "nvarchar(max)",
                table: "TaskLists",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "nvarchar(80)",
                table: "TaskLists",
                newName: "Title");
        }
    }
}
