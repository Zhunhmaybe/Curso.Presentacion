using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Entidades
{
    public class Usuarios
    {
        [Key]
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string ContraseñaHash { get; set; }
        public DateTime FechaRegistro { get; set; } 

        //relaciones
        public List<ListasTareas> ListasTareas { get; set; }



    }
}
