using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BENT1C.Grupo5.Migrations
{
    public partial class SegundaVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrador",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 60, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    FechaAlta = table.Column<DateTime>(nullable: false),
                    Password = table.Column<byte[]>(nullable: true),
                    Legajo = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrador", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrador");
        }
    }
}
