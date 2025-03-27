using Curso.ConsumeAPI;
using Curso.Entidades;
using Curso.Servicios.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Curso.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Curso.Presentacion.Controllers
{
    [Authorize]
    public class ListasTareasController : Controller
    {
        private readonly IApiService _aPIService;
        private readonly Data.Dbcontext _dbContext;
        public ListasTareasController(IApiService aPIService, Data.Dbcontext dbContext)
        {
            _aPIService = aPIService;
            _dbContext = dbContext;
        }
        // GET: ListasTareasController
        public async Task<ActionResult> Index()
        {
            string userNameClain = User.FindFirst(ClaimTypes.Name).Value;
            var Usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(o => o.Email == userNameClain);

            int id = Usuario.UsuarioID;
            var data = CRUD<ListasTareas>.Read(_aPIService.getApiUrl()+"/ListasTareas/ByID"+id);//controlar aqui siempre las rutas y siempre publicas si cambias algo a la api

            return View(data);
        }

        // GET: ListasTareasController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ListasTareasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ListasTareasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ListasTareasController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ListasTareasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ListasTareasController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ListasTareasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
