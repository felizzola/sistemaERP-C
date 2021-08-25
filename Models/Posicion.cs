using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENT1C.Grupo5.Entidades
{
    // TODO: Todas las propiedades de tipo string tienen que tener al menos MaxLength
    public class Posicion
    {
        [Key]
        public Guid Id { get; set; }

        [Required (ErrorMessage ="Por favor ingrese un nombre")]
        [MaxLength(15, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Por favor ingrese una descripcion")]
        [MaxLength(200, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Por favor aclare un sueldo.")]
        public decimal Sueldo { get; set; }

        [ForeignKey(nameof(Empleado))]
        public Guid EmpleadoId { get; set; }
        public Empleado Empleado { get; set; }

        [ForeignKey(nameof(Responsable))]
        public Guid? ResponsableId { get; set; }
        public Posicion Responsable { get; set; }

        [ForeignKey(nameof(Gerencia))]
        public Guid GerenciaId { get; set; }
        public Gerencia Gerencia { get; set; }
    }
}
