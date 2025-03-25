using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Curso.Entidades;

namespace Curso.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ListasTareasController : Controller
    {
        private readonly Dbcontext _context;

        public ListasTareasController(Dbcontext context)
        {
            _context = context;
        }

        // GET: ListasTareas
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listaTareas = await _context.ListasTareas.ToListAsync();
            return Ok(listaTareas);
        }

        // GET: ListasTareas/Details/5
        [HttpGet("TaskListByID/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var listasTareas = await _context.Usuarios
                    .Include(l => l.ListasTareas)
                    .ThenInclude(t => t.Tareas)                    
                    .FirstOrDefaultAsync(m => m.UsuarioID == id);

            if (listasTareas == null)
            {
                return BadRequest();
            }

            return Ok(listasTareas);
        }


        // POST: ListasTareas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Crear")]
        public async Task<IActionResult> Create( ListasTareas listasTareas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listasTareas);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Se creo el usuario", listasTareas = listasTareas });
            }
            return BadRequest(new { Message = "Error al crear el usuario " });
        }

        // GET: ListasTareas/Edit/5
        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id,ListasTareas listasTareas)
        {
            if (id != listasTareas.ListaID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listasTareas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListasTareasExists(listasTareas.ListaID))
                    {
                        return BadRequest();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(new { Message = "Se Edit Lista de Tareas" });
            }            
            return BadRequest();
        }



        // GET: ListasTareas/Delete/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            var listasTareas = await _context.ListasTareas.FindAsync(id);
            if (listasTareas != null)
            {
                _context.ListasTareas.Remove(listasTareas);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "se borro", listasTareas = listasTareas });
        }
        



        private bool ListasTareasExists(int id)
        {
            return _context.ListasTareas.Any(e => e.ListaID == id);
        }
    }
}
