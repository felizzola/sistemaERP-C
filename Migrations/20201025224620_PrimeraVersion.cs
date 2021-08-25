using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BENT1C.Grupo5.Migrations
{
    public partial class PrimeraVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Imagenes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 200, nullable: false),
                    Url = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoTelefonos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoTelefonos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 60, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    FechaAlta = table.Column<DateTime>(nullable: false),
                    Password = table.Column<byte[]>(nullable: true),
                    Apellido = table.Column<string>(maxLength: 60, nullable: false),
                    Dni = table.Column<string>(maxLength: 15, nullable: true),
                    ObraSocial = table.Column<string>(maxLength: 60, nullable: false),
                    Legajo = table.Column<int>(nullable: false),
                    EmpleadoActivo = table.Column<bool>(nullable: false),
                    FotoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empleados_Imagenes_FotoId",
                        column: x => x.FotoId,
                        principalTable: "Imagenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    Rubro = table.Column<string>(nullable: false),
                    Direccion = table.Column<string>(nullable: true),
                    TelefonoContacto = table.Column<string>(nullable: false),
                    EmailContacto = table.Column<string>(nullable: true),
                    LogoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empresas_Imagenes_LogoId",
                        column: x => x.LogoId,
                        principalTable: "Imagenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Telefonos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Numero = table.Column<string>(maxLength: 15, nullable: true),
                    TipoTelefonoId = table.Column<Guid>(nullable: false),
                    EmpleadoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefonos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Telefonos_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Telefonos_TipoTelefonos_TipoTelefonoId",
                        column: x => x.TipoTelefonoId,
                        principalTable: "TipoTelefonos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gerencias",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 200, nullable: false),
                    EsGerenciaGeneral = table.Column<bool>(nullable: false),
                    DireccionId = table.Column<Guid>(nullable: true),
                    ResponsableId = table.Column<Guid>(nullable: false),
                    EmpresaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gerencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gerencias_Gerencias_DireccionId",
                        column: x => x.DireccionId,
                        principalTable: "Gerencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gerencias_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gerencias_Empleados_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CentroDeCostos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 100, nullable: false),
                    MontoMaximo = table.Column<decimal>(nullable: false),
                    GerenciaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentroDeCostos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CentroDeCostos_Gerencias_GerenciaId",
                        column: x => x.GerenciaId,
                        principalTable: "Gerencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posiciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    Descripcion = table.Column<string>(nullable: false),
                    Sueldo = table.Column<decimal>(nullable: false),
                    EmpleadoId = table.Column<Guid>(nullable: false),
                    ResponsableId = table.Column<Guid>(nullable: false),
                    GerenciaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posiciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posiciones_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posiciones_Gerencias_GerenciaId",
                        column: x => x.GerenciaId,
                        principalTable: "Gerencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posiciones_Posiciones_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "Posiciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Descripcion = table.Column<string>(maxLength: 500, nullable: false),
                    Monto = table.Column<decimal>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    CentroDeCostoId = table.Column<Guid>(nullable: false),
                    EmpleadoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gastos_CentroDeCostos_CentroDeCostoId",
                        column: x => x.CentroDeCostoId,
                        principalTable: "CentroDeCostos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gastos_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CentroDeCostos_GerenciaId",
                table: "CentroDeCostos",
                column: "GerenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_FotoId",
                table: "Empleados",
                column: "FotoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_LogoId",
                table: "Empresas",
                column: "LogoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_CentroDeCostoId",
                table: "Gastos",
                column: "CentroDeCostoId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_EmpleadoId",
                table: "Gastos",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_DireccionId",
                table: "Gerencias",
                column: "DireccionId");

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_EmpresaId",
                table: "Gerencias",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_ResponsableId",
                table: "Gerencias",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Posiciones_EmpleadoId",
                table: "Posiciones",
                column: "EmpleadoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posiciones_GerenciaId",
                table: "Posiciones",
                column: "GerenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Posiciones_ResponsableId",
                table: "Posiciones",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_EmpleadoId",
                table: "Telefonos",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_TipoTelefonoId",
                table: "Telefonos",
                column: "TipoTelefonoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gastos");

            migrationBuilder.DropTable(
                name: "Posiciones");

            migrationBuilder.DropTable(
                name: "Telefonos");

            migrationBuilder.DropTable(
                name: "CentroDeCostos");

            migrationBuilder.DropTable(
                name: "TipoTelefonos");

            migrationBuilder.DropTable(
                name: "Gerencias");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Imagenes");
        }
    }
}
