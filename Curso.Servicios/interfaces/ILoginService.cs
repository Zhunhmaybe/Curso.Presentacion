using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Servicios.interfaces
{
    public interface ILoginService 
    {
        Task<bool> RegisterService(string Usuario, string Email, string contrasena, string confcontrasena);
        Task<bool> PostLogin(string Contraseña, string Email);
    }
}
