using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Curso.Entidades;
using Curso.Data;

namespace Curso.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListasTareasController : ControllerBase
    {
        private readonly Dbcontext _context;

        public ListasTareasController(Dbcontext context)
        {
            _context = context;
        }

        // GET: api/ListasTareas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListasTareas>>> GetListasTareas()
        {
            return await _context.ListasTareas.ToListAsync();
        }


        // GET: api/ListasTareas/5
        [HttpGet("ByID{idUser}")]
        public async Task<IActionResult> ListByUser(int idUser)
        {
            var listasTareas = await _context.ListasTareas
            .Include(t => t.Tareas)
            .Where(u => u.UsuarioID == idUser)
            .Select(item => new
            {
                item.ListaID,
                item.UsuarioID,
                item.Nombre,
                Tareas = item.Tareas.Select(t => new
                {
                    t.TareaID,
                    t.Descripcion,
                    t.Estado
                }),
                Usuario = new
                {
                    item.Usuarios.UsuarioID,
                    item.Usuarios.Nombre
                }
            })
            .ToListAsync();
            return Ok(listasTareas);
        }

        // PUT: api/ListasTareas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutListasTareas(int id, ListasTareas listasTareas)
        {
            if (id != listasTareas.ListaID)
            {
                return BadRequest();
            }

            _context.Entry(listasTareas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListasTareasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ListasTareas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ListasTareas>> PostListasTareas(ListasTareas listasTareas)
        {
            _context.ListasTareas.Add(listasTareas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetListasTareas", new { id = listasTareas.ListaID }, listasTareas);
        }

        // DELETE: api/ListasTareas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListasTareas(int id)
        {
            var listasTareas = await _context.ListasTareas.FindAsync(id);
            if (listasTareas == null)
            {
                return NotFound();
            }

            _context.ListasTareas.Remove(listasTareas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ListasTareasExists(int id)
        {
            return _context.ListasTareas.Any(e => e.ListaID == id);
        }
    }
}
