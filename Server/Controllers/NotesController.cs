#nullable disable
using AutoMapper;
using AutoMapper.AspNet.OData;
using ClinicProject.Server.Data;
using ClinicProject.Server.Data.DBModels.NotesTypes;
using ClinicProject.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ClinicProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ODataController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public NotesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<IActionResult> GetNotes(ODataQueryOptions<NoteDTO> options)
        {
            var query = await _context.Notes.GetQueryAsync<NoteDTO, Note>(mapper, options);
            return Ok(await query.ToListAsync());
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote([FromODataUri] int id, ODataQueryOptions<NoteDTO> options)
        {
            var result = await _context.Notes.Where(p => p.Id == id).GetQueryAsync(mapper, options);
            return Ok(await result.FirstOrDefaultAsync());
        }

        // PUT: api/Notes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableQuery]
        public async Task<IActionResult> PutNote([FromODataUri] int id, [FromODataBody] NoteDTO noteDTO)
        {
            if (!ModelState.IsValid || id != noteDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(mapper.Map<Note>(noteDTO)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
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

        // POST: api/Notes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        public async Task<ActionResult<NoteDTO>> PostNote([FromODataBody] NoteDTO noteDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _context.Notes.AddAsync(mapper.Map<Note>(noteDTO));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = noteDTO.Id }, noteDTO);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        [EnableQuery]
        public async Task<IActionResult> DeleteNote([FromODataUri] int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
