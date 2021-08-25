using BENT1C.Grupo5.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENT1C.Grupo5.Entidades
{
    // Se remueve el Id ya que dicha propiedad se hereda de Usuario.
    // Recordar no poner validaciones a los atributos de tipo "clase de usuario", por ejemplo en 
    // este caso se remueve el de Telefonos ya que se trata de una Lista de Telefono.
    public class Empleado : Usuario
    {
        [Required(ErrorMessage = "Debe ingresar Apellido")]
        [MaxLength(60, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Apellido { get; set; }

        [RegularExpression(@"[0-9]{2}\.[0-9]{3}\.[0-9]{3}", ErrorMessage = "El formato del Dni debera ser EJ: NN.NNN.NNN")]
        [MaxLength(15, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "Por favor aclare una obra social.")]
        [MaxLength(60, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string ObraSocial { get; set; }

        [Required(ErrorMessage = "Por favor aclare un legajo.")]
        public int Legajo { get; set; }

        public bool EmpleadoActivo { get; set; }
        public bool EmpleadoRrhh { get; set; }

        // [ForeignKey(nameof(Imagen))] no está bien ya que el nameof debe ser el nombre de la propiedad de navegación, en este caso Foto
        [ForeignKey(nameof(Foto))]
        public Guid FotoId { get; set; }
        public Imagen Foto { get; set; }

        // Relación 1 a 1. La FK está en posición.
        public Posicion Posicion { get; set; }

        public List<Gasto> Gastos { get; set; }

        public List<Telefono> Telefonos { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";
        public override Rol Rol => Rol.Empleado;
    }
}
