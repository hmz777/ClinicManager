#nullable disable
using AutoMapper;
using AutoMapper.AspNet.OData;
using ClinicProject.Server.Data;
using ClinicProject.Server.Data.DBModels.TreatmentTypes;
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
    public class TreatmentsController : ODataController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public TreatmentsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Treatments
        [HttpGet]
        public async Task<IActionResult> GetTreatments(ODataQueryOptions<TreatmentDTO> options)
        {
            var query = await _context.Treatments.GetQueryAsync<TreatmentDTO, Treatment>(mapper, options);
            return Ok(await query.ToListAsync());
        }

        // GET: api/Treatments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTreatment([FromODataUri] int id, ODataQueryOptions<TreatmentDTO> options)
        {
            var result = await _context.Treatments.Where(p => p.Id == id).GetQueryAsync(mapper, options);
            return Ok(await result.FirstOrDefaultAsync());
        }

        // PUT: api/Treatments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableQuery]
        public async Task<IActionResult> PutTreatment([FromODataUri] int id, [FromODataBody] TreatmentDTO treatmentDTO)
        {
            if (!ModelState.IsValid || id != treatmentDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(mapper.Map<Treatment>(treatmentDTO)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TreatmentExists(id))
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

        // POST: api/Treatments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        public async Task<ActionResult<TreatmentDTO>> PostTreatment([FromODataBody] TreatmentDTO treatmentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _context.Treatments.AddAsync(mapper.Map<Treatment>(treatmentDTO));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTreatment", new { id = treatmentDTO.Id }, treatmentDTO);
        }

        // DELETE: api/Treatments/5
        [HttpDelete("{id}")]
        [EnableQuery]
        public async Task<IActionResult> DeleteTreatment([FromODataUri] int id)
        {
            var treatment = await _context.Treatments.FindAsync(id);
            if (treatment == null)
            {
                return NotFound();
            }

            _context.Treatments.Remove(treatment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TreatmentExists(int id)
        {
            return _context.Treatments.Any(e => e.Id == id);
        }
    }
}
