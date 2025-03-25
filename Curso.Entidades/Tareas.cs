using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Entidades
{
   public class Tareas
    {
        [Key]
        public int TareaID { get; set; }
        public int ListaID { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Estado { get; set; }

        //relaciones
        [ForeignKey("ListaID")]

        public ListasTareas? ListasTarea { get; set; }
        public Recordatorios? Recordatorios { get; set; }
    }
}
