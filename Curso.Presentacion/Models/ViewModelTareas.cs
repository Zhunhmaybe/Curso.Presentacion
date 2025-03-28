using Curso.Entidades;
namespace Curso.Presentacion.Models
{
    public class ViewModelTareas
    {
        public int TareaID{ get; set; }
        public int ListaID { get; set; }
        public string Description { get; set; }
        public DateTime FechaVencimiento { get; set; }

    }
}
