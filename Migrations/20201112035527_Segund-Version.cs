using Microsoft.EntityFrameworkCore.Migrations;

namespace BENT1C.Grupo5.Migrations
{
    public partial class SegundVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmpleadoRrhh",
                table: "Empleados",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmpleadoRrhh",
                table: "Empleados");
        }
    }
}
