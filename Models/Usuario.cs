using BENT1C.Grupo5.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace BENT1C.Grupo5.Entidades
{
    public abstract class Usuario
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar Nombre")]
        [MaxLength(60, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "{0} admite un máximo de {1} caracteres")]
        [EmailAddress(ErrorMessage = "El campo {0} debe ser un email válido")]
        public string Email { get; set; }

        public DateTime FechaAlta { get; set; }

        [ScaffoldColumn(false)] // Utilizamos esto para que no se autogenere el campo password cuando hacemos scaffolding
        public byte[] Password { get; set; } // La password es de tipo array de bytes para almacenar las contraseñas encriptadas

        public abstract Rol Rol { get; }

    }
}
