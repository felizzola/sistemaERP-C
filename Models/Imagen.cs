using System;
using System.ComponentModel.DataAnnotations;

namespace BENT1C.Grupo5.Entidades
{
    public class Imagen
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Por favor aclare un nombre.")]
        [MaxLength(200, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es mandatorio")]
        [MaxLength(200, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        [Url(ErrorMessage = "El campo {0} debe ser una url válida")]
        public string Url { get; set; }

        // Puede estar relacionada a un empleado o a una empresa, pero como la relación en ambos casos es 1 a 1, 
        // establecemos las FK en empleado y empresa respectivamente.
        public Empleado Empleado { get; set; }
        public Empresa Empresa { get; set; }
    }
}
