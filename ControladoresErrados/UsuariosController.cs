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
    public class UsuariosController : Controller
    {
        private readonly Dbcontext _context;

        public UsuariosController(Dbcontext context)
        {
            _context = context;
        }

        // GET: Usuarios
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarios=await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        // GET: Usuarios/Details/5
        [HttpGet("UserByID/{id}")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();//ponemos bad request
            }

                var usuarios = await _context.Usuarios
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
            if (usuarios == null)
            {
                return BadRequest();
            }

            return Ok(usuarios);
        }

        // GET: Usuarios/Create
        /*
        public IActionResult Create()
        {
            return View();
        }
        */

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Crear")]        
        public async Task<IActionResult> Create(Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarios);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Se creo el usuario", usuario = usuarios });
            }
            return BadRequest(new {Message = "Error al crear el usuario "});
        }


        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id,Usuarios usuarios)
        {
            if (id != usuarios.UsuarioID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuariosExists(usuarios.UsuarioID))
                    {
                        return BadRequest();
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
        

        // GET: Usuarios/Delete/5
        /*
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioID == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }
        */

        // POST: Usuarios/Delete/5
        
        [HttpPost("Delete/{id}")] 
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios != null)
            {
                _context.Usuarios.Remove(usuarios);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "se borro", usuario = usuarios });
        }

        private bool UsuariosExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioID == id);
        }
        
    }
}
