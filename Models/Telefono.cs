using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENT1C.Grupo5.Entidades
{
    public class Telefono
    {
        [Key]
        public Guid Id {get; set;}

        [RegularExpression(@"[0-9]{3}\-[0-9]{4}\-[0-9]{4}", ErrorMessage = "El formato es invalido")]
        [MaxLength(15, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Numero {get; set;}

        [ForeignKey(nameof(TipoTelefono))]
        public Guid TipoTelefonoId { get; set; }
        public TipoTelefono TipoTelefono { get; set; }

        [ForeignKey(nameof(Empleado))]
        public Guid EmpleadoId { get; set; }
        public Empleado Empleado { get; set; }
    }
}
