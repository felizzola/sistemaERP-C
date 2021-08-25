using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BENT1C.Grupo5.Entidades
{
    public class Gerencia
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Por favor ingrese un nombre.")]
        [MaxLength(50, ErrorMessage = "El campo {0} admite un máximo de {1} caracteres")]
        public string Nombre { get; set; }

        public bool EsGerenciaGeneral { get; set; }

        [ForeignKey(nameof(Direccion))]
        public Guid? DireccionId { get; set; }
        public Gerencia Direccion { get; set; }

        [ForeignKey(nameof(Responsable))]
        public Guid ResponsableId { get; set; }
        public Empleado Responsable { get; set; }

        public List<Posicion> Posiciones { get; set; }

        public List<Gerencia> Gerencias { get; set; }

        [ForeignKey(nameof(Empresa))]
        public Guid EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        public List<CentroDeCosto> CentroDeCostos { get; set; }
    }
}
