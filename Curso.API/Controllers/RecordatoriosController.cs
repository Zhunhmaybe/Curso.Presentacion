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
    public class RecordatoriosController : ControllerBase
    {
        private readonly Dbcontext _context;

        public RecordatoriosController(Dbcontext context)
        {
            _context = context;
        }

        // GET: api/Recordatorios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recordatorios>>> GetRecordatorios()
        {
            return await _context.Recordatorios.ToListAsync();
        }

        // GET: api/Recordatorios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recordatorios>> GetRecordatorios(int id)
        {
            var recordatorios = await _context.Recordatorios.FindAsync(id);

            if (recordatorios == null)
            {
                return NotFound();
            }

            return recordatorios;
        }

        // PUT: api/Recordatorios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecordatorios(int id, Recordatorios recordatorios)
        {
            if (id != recordatorios.RecordatorioID)
            {
                return BadRequest();
            }

            _context.Entry(recordatorios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordatoriosExists(id))
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

        // POST: api/Recordatorios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recordatorios>> PostRecordatorios(Recordatorios recordatorios)
        {
            _context.Recordatorios.Add(recordatorios);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecordatorios", new { id = recordatorios.RecordatorioID }, recordatorios);
        }

        // DELETE: api/Recordatorios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecordatorios(int id)
        {
            var recordatorios = await _context.Recordatorios.FindAsync(id);
            if (recordatorios == null)
            {
                return NotFound();
            }

            _context.Recordatorios.Remove(recordatorios);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecordatoriosExists(int id)
        {
            return _context.Recordatorios.Any(e => e.RecordatorioID == id);
        }
    }
}
