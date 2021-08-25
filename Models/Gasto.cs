using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENT1C.Grupo5.Entidades
{
    public class Gasto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Por favor agregue una descripción.")]
        [MaxLength(500, ErrorMessage = "La descripción tiene como máximo {1} caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Por favor aclare un monto.")]
        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }

        [ForeignKey(nameof(CentroDeCosto))]
        public Guid CentroDeCostoId { get; set; }
        public CentroDeCosto CentroDeCosto { get; set; }

        [ForeignKey(nameof(Empleado))]
        public Guid EmpleadoId { get; set; }
        public Empleado Empleado { get; set; }
    }
}
