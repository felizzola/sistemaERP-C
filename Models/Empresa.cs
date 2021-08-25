using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENT1C.Grupo5.Entidades
{
    // TODO: Todas las propiedades de tipo string tienen que tener al menos MaxLength
    public class Empresa
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        [Required(ErrorMessage = "Please enter Name")]
        public string Nombre { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        [Required(ErrorMessage = "Por favor aclare un rubro.")]
        public string Rubro { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Por favor aclare un telefono.")]
        [MaxLength(100, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string TelefonoContacto { get; set; }

        [EmailAddress(ErrorMessage = "Debe ingresar un email valido")]
        [MaxLength(100, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string EmailContacto { get; set; }

        [ForeignKey(nameof(Logo))]
        public Guid LogoId { get; set; }
        public Imagen Logo { get; set; }

        public List<Gerencia> Gerencias { get; set; }
    }
}
