using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CertifacAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CertifacAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddendasController : ControllerBase
    {
        public readonly cer_addendasContext _context;

        public AddendasController(cer_addendasContext context)
        {
            _context = context;
        }

        // GET: api/<adendasController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Addenda>>> GetAddendas()
        {
            if (_context.Addendas == null)
            {
                return NotFound();
            }
            return await _context.Addendas.ToListAsync();
        }

        // GET api/<adendasController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Addenda>> GetAddenda(Guid id)
        {
            if (_context.Addendas == null)
            {
                return NotFound();
            }
            var addendas = await _context.Addendas.FindAsync(id);

            if (addendas == null)
            {
                return NotFound();
            }

            return addendas;
        }

        // POST api/<adendasController>
        [HttpPost]
        public async Task<ActionResult<Addenda>> PostAddenda(Addenda addenda)
        {
            if (_context.Addendas == null)
            {
                return Problem("Entity set 'EstatusContext.Addendas'  is null.");
            }
            _context.Addendas.Add(addenda);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAddendas", new { id = addenda.IdAddenda }, addenda);
        }

        // PUT api/<adendasController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddenda(Guid id, Addenda addenda)
        {
            if (id != addenda.IdAddenda)
            {
                return BadRequest();
            }

            _context.Entry(addenda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddendaExists(id))
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

        // DELETE api/<adendasController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddenda(Guid id)
        {
            if (_context.Addendas == null)
            {
                return NotFound();
            }
            var addenda = await _context.Addendas.FindAsync(id);
            if (addenda == null)
            {
                return NotFound();
            }

            _context.Addendas.Remove(addenda);
            await _context.SaveChangesAsync();

            return StatusCode(0);
        }

        
        [HttpDelete("deleteMultiple/{ids}")]
        public async Task<IActionResult> DeleteAddendas(string ids)
        {
            List<string> guids = ids.Split(',').ToList();
            if (_context.Addendas == null)
            {
                return NotFound();
            }
            foreach(var id in guids)
            {
                var addenda = await _context.Addendas.FindAsync(new Guid (id));
                if (addenda == null)
                {
                    return NotFound();
                }

                _context.Addendas.Remove(addenda);
            }
            
            await _context.SaveChangesAsync();

            return StatusCode(0);
        }

        [HttpGet("dateRange/{date1}/{date2}")]
        public async Task<ActionResult<IEnumerable<Addenda>>> GetAddendasByRange(DateTime date1, DateTime date2)
        {
            if (_context.Addendas == null)
            {
                return NotFound();
            }
            var addendas = await _context.Addendas.Where(x => x.FechaModificacion >= date1 && x.FechaModificacion <= date2 ).ToListAsync();

            if (addendas == null)
            {
                return NotFound();
            }

            return addendas;
        }

        [HttpPost("insertMultiple")]
        public async Task<ActionResult<Addenda>> PostAddenda(List<Addenda> addendas)
        {
            if (_context.Addendas == null)
            {
                return Problem("Entity set 'EstatusContext.Addendas'  is null.");
            }
            foreach(var addenda in addendas)
            {
                _context.Addendas.Add(addenda);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetAddendas", addendas);
        }

        private bool AddendaExists(Guid id)
        {
            return (_context.Addendas?.Any(e => e.IdAddenda == id)).GetValueOrDefault();
        }
    }
}
