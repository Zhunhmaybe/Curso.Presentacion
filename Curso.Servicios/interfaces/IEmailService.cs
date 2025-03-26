using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Servicios.interfaces
{
    public interface IEmailService
    {
       void SendEmail(string Para, string Asunto, string Contenido);
    }
}
