using BENT1C.Grupo5.Entidades;
using Microsoft.EntityFrameworkCore;
using BENT1C.Grupo5.Models;

namespace BENT1C.Grupo5.Database
{
    public class ErpDbContext : DbContext
    {
        //cuando creemos la instancia de dbcontext le pasamos la configuracion, osea, le decimos
        //que motor, y a que base de datos me conecto.
        public ErpDbContext(DbContextOptions<ErpDbContext> options) : base(options)
        {
        }


        //son nuestras clases representadas en la base de datos como tablas
        //mi db context tiene una tabla llamada CentroDeCostos que es un set de datos de tipo CentroDeCosto (DbSet<CentroDeCosto>)
        //aka DbSet<Clase> NombreDeClase
        //las columnas adentro de nuestra tabla CentroDeCostos estan dictadas por los atributos de las clases.

        public DbSet<CentroDeCosto> CentroDeCostos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Gerencia> Gerencias { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }
        public DbSet<Posicion> Posiciones { get; set; }
        public DbSet<Telefono> Telefonos { get; set; }
        public DbSet<TipoTelefono> TipoTelefonos { get; set; }
        public DbSet<BENT1C.Grupo5.Models.Administrador> Administrador { get; set; }
    }
}
