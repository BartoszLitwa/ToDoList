using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDoList.Infrastructure.Migrations
{
    public partial class ProperlySetTaskListIdForTask2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tasks",
                type: "nvarchar(80)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TaskLists",
                type: "nvarchar(80)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "nvarchar(80)",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nvarchar(80)",
                table: "TaskLists",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldNullable: true);
        }
    }
}
