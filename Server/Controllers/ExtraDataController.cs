#nullable disable
using AutoMapper;
using AutoMapper.AspNet.OData;
using ClinicProject.Server.Data;
using ClinicProject.Server.Data.DBModels.PatientTypes;
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
    public class ExtraDataController : ODataController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public ExtraDataController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/ExtraDatas
        [HttpGet]
        public async Task<IActionResult> GetExtraDatas(ODataQueryOptions<ExtraDataDTO> options)
        {
            var query = await _context.ExtraDatas.GetQueryAsync<ExtraDataDTO, ExtraData>(mapper, options);
            return Ok(await query.ToListAsync());
        }

        // GET: api/ExtraDatas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExtraData([FromODataUri] int id, ODataQueryOptions<ExtraDataDTO> options)
        {
            var result = await _context.ExtraDatas.Where(p => p.Id == id).GetQueryAsync(mapper, options);
            return Ok(await result.FirstOrDefaultAsync());
        }

        // PUT: api/ExtraDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableQuery]
        public async Task<IActionResult> PutExtraData([FromODataUri] int id, [FromODataBody] ExtraDataDTO extraDataDTO)
        {
            if (!ModelState.IsValid || id != extraDataDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(mapper.Map<ExtraData>(extraDataDTO)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExtraDataExists(id))
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

        // POST: api/ExtraDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        public async Task<ActionResult<ExtraDataDTO>> PostExtraData([FromODataBody] ExtraDataDTO extraDataDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _context.ExtraDatas.AddAsync(mapper.Map<ExtraData>(extraDataDTO));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExtraData", new { id = extraDataDTO.Id }, extraDataDTO);
        }

        // DELETE: api/ExtraDatas/5
        [HttpDelete("{id}")]
        [EnableQuery]
        public async Task<IActionResult> DeleteExtraData([FromODataUri] int id)
        {
            var extraData = await _context.ExtraDatas.FindAsync(id);
            if (extraData == null)
            {
                return NotFound();
            }

            _context.ExtraDatas.Remove(extraData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExtraDataExists(int id)
        {
            return _context.ExtraDatas.Any(e => e.Id == id);
        }
    }
}
