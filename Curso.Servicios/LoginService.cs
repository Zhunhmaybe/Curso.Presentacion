using Curso.Data;
using Curso.Servicios.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Servicios
{
    public class LoginService : ILoginService
    {
        //Inyeccion de dependencia
        private readonly Curso.Data.Dbcontext _dbcontext;
        public LoginService(Curso.Data.Dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<bool> RegisterService(string Usuario, string Email, string contrasena, string confcontrasena)
        {

            //Se le puede agregar el usuario 
            //esto sirve para ver si hay emails repetidos
            var user = await _dbcontext.Usuarios.FirstOrDefaultAsync(x => x.Email == Email);
            //Osea si exite en la base 
            if (user != null) return false;

            //Contrsena iguales 

            if (contrasena != confcontrasena) return false;

            var NewUser = new Curso.Entidades.Usuarios
            {
                Nombre = Usuario,
                Email = Email,
                ContraseñaHash = BCrypt.Net.BCrypt.HashPassword(confcontrasena),
                FechaRegistro = DateTime.Now
            };
            _dbcontext.Usuarios.Add(NewUser);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PostLogin(string Contraseña, string Email)
        {
            var user = await _dbcontext.Usuarios.FirstOrDefaultAsync(x => x.Email == Email);
            if (user == null) return false;
            return BCrypt.Net.BCrypt.Verify(Contraseña, user.ContraseñaHash);

        }


    }
}
