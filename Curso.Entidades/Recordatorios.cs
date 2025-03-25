using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Entidades
{
    public class Recordatorios
    {
        [Key]
        public int RecordatorioID { get; set; }
        public int TareaID { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool Enviado { get; set; }
        public string EmailUsuario { get; set; }

        //relaciones
        [ForeignKey("TareaID")]
        public Tareas? Tarea { get; set; }

    }
}
