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
    public class RecordatoriosController : Controller
    {
        private readonly Dbcontext _context;

        public RecordatoriosController(Dbcontext context)
        {
            _context = context;
        }

        // GET: Recordatorios
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var recordatorios = await _context.Recordatorios.ToListAsync();
            return Ok(recordatorios);
        }

        // GET: Recordatorios/Details/5
        [HttpGet("ReminderByID/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var recordatorios = await _context.Recordatorios
                .Include(r => r.Tarea)
                .Where(l=>l.TareaID==id)
                .Select(item => new { 
                    item.RecordatorioID,
                    item.TareaID,
                    item.FechaEnvio,
                    item.Enviado,
                    item.EmailUsuario
                })
                .FirstOrDefaultAsync(m => m.RecordatorioID == id);

            if (recordatorios == null)
            {
                return BadRequest();
            }

            return Ok(recordatorios);
        }

        // GET: Recordatorios/Create
        [HttpPost("Crear")]
        public async Task<IActionResult> Create(Recordatorios recordatorios)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recordatorios);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Se creo el recordatorio", recordatorio = recordatorios});
            }
            return BadRequest(new { Message = "Error al crear recordatorio " });
        }

        // GET: Recordatorios/Edit/5
        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id,Recordatorios recordatorios)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recordatorios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordatoriosExists(recordatorios.RecordatorioID))
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



        // POST: Recordatorios/Delete/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recordatorios = await _context.Recordatorios.FindAsync(id);
            if (recordatorios != null)
            {
                _context.Recordatorios.Remove(recordatorios);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "se borro", recordatorio = recordatorios });
        }

        private bool RecordatoriosExists(int id)
        {
            return _context.Recordatorios.Any(e => e.RecordatorioID == id);
        }
    }
}
