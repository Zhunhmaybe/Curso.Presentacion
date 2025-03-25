using AspNetCoreHero.ToastNotification.Abstractions;
using Curso.Servicios.interfaces;
using Curso.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Curso.Presentacion.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Curso.Presentacion.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        public INotyfService _notifyService { get; }
        public LoginController(ILoginService loginService, INotyfService notifyService)
        {
            _loginService = loginService;
            _notifyService = notifyService;
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: LoginController/Details/5
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PostRegister(RegistroViewModel Data)
        {
            bool resul = await _loginService.RegisterService(Data.Usuario, Data.Email, Data.Contrasena, Data.ConfirmarContrasena);
            if (resul == false)
            {
                _notifyService.Error("Usuario Registrado");
                return RedirectToAction("Register");
            }
            _notifyService.Success($"Hola {Data.Usuario}");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> PostLogin(ViewModelLogin Data)
        {
            bool result = await _loginService.PostLogin(Data.Contrasena, Data.Usuario);
            if (result == false)
            {
                _notifyService.Error("Usuario o Contraseña Incorrectos");
                return RedirectToAction("Index");
            }
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, Data.Usuario),
            // new Claim(ClaimTypes.Role, rol)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            _notifyService.Success("Bienvenido a la plataforma");
            return RedirectToAction("Privacy", "Home");
        }


    }
}
