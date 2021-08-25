using BENT1C.Grupo5.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BENT1C.Grupo5.Models
{
    public class Administrador : Usuario
    {
        [ScaffoldColumn(false)]
        public Guid Legajo { get; set; }
        public override Rol Rol =>  Rol.Administrador;
    }

}
