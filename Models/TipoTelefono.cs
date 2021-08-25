using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BENT1C.Grupo5.Entidades
{
    public class TipoTelefono
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Por favor ingrese un tipo.")]
        [MaxLength(100, ErrorMessage = "El campo Tipo de teléfono admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        public List<Telefono> Telefonos { get; set; }
    }
}
