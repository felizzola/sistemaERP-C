using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENT1C.Grupo5.Entidades
{
    public class CentroDeCosto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Por favor aclare un nombre.")]
        [MaxLength(100, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Por favor aclare un monto maximo.")]
        public decimal MontoMaximo { get; set; }

        [ForeignKey(nameof(Gerencia))]
        public Guid GerenciaId { get; set; }
        public Gerencia Gerencia { get; set; }

        public List<Gasto> Gastos { get; set; }
    }
}
