using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Curso.Entidades;
using System.Threading;

namespace Curso.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TareasController : Controller
    {
        private readonly Dbcontext _context;

        public TareasController(Dbcontext context)
        {
            _context = context;
        }

        // GET: Tareas
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tareas =await _context.Tareas.ToListAsync();
            return Ok(tareas);
        }

        // GET: Tareas/Details/5
        [HttpGet("TaskByID/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var tareas = await _context.Usuarios
                    .Include(l => l.ListasTareas)
                    .ThenInclude(t => t.Tareas)
                    .Select(item => new
                    {
                        item.UsuarioID,
                        item.Nombre,
                        item.Email,
                        item.FechaRegistro,
                        ListaTareas = item.ListasTareas.Select(l => new
                        {
                            l.ListaID,
                            l.Nombre,
                            Tareas = l.Tareas.Select(t => new
                            {
                                t.TareaID,
                                t.Descripcion,
                                t.Estado
                            })
                        })

                    })
                    .FirstOrDefaultAsync(m => m.UsuarioID == id);


            if (tareas == null)
            {
                return BadRequest();
            }

            return Ok(tareas);
        }
        
        // GET: Tareas/Create
        [HttpPost("Crear")]
        public async Task<IActionResult> Create(Tareas tareas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tareas);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Se creo la tarea", tareas = tareas});
            }
            return BadRequest(new { Message = "Error al crear tarea" });
        }

        // GET: Tareas/Edit/

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id,Tareas tareas)
        {
            if (id != tareas.TareaID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tareas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TareasExists(tareas.TareaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(new { Message = "Se Edito" });
            }
            return BadRequest();
        }


        // POST: Tareas/Delete/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tareas = await _context.Tareas.FindAsync(id);
            if (tareas != null)
            {
                _context.Tareas.Remove(tareas);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "se borro", tareas = tareas });
        }
        
        private bool TareasExists(int id)
        {
            return _context.Tareas.Any(e => e.TareaID == id);
        }
        
    }
}
