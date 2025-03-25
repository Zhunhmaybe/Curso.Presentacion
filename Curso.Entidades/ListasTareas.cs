using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Entidades
{
    public class ListasTareas
    {
        [Key]
        public int ListaID { get; set; }
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }

        //relaciones
        [ForeignKey("UsuarioID")]
        public Usuarios? Usuarios { get; set; }
        public List<Tareas>? Tareas { get; set; }
    }
}

    