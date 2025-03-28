using Curso.Entidades;

namespace Curso.Presentacion.Models
{
    public class ViewModelListasTareas
    {
        public int ListaID { get; set; }
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string nombreUsuario { get; set; }
        public List<Tareas> tareas { get; set; }
    }
}
