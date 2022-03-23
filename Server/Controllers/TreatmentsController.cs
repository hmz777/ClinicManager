#nullable disable
using AutoMapper;
using AutoMapper.AspNet.OData;
using ClinicProject.Server.Data;
using ClinicProject.Server.Data.DBModels.PatientTypes;
using ClinicProject.Server.Data.DBModels.TreatmentTypes;
using ClinicProject.Shared.DTOs;
using ClinicProject.Shared.Models.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ClinicProject.Server.Controllers
{
    public class TreatmentsController : ODataController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public TreatmentsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(ODataQueryOptions<TreatmentDTO> options)
        {
            var query = await _context.Treatments.GetQueryAsync<TreatmentDTO, Treatment>(mapper, options);
            return Ok(await query.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Get(int key, ODataQueryOptions<TreatmentDTO> options)
        {
            var result = await _context.Treatments.Where(p => p.Id == key).GetQueryAsync(mapper, options);
            return Ok(await result.FirstOrDefaultAsync());
        }

        [HttpPut]
        [EnableQuery]
        public async Task<IActionResult> Put(int key, [FromBody] TreatmentDTO treatmentDTO)
        {
            if (key != treatmentDTO.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                var ValidationErrors = new ModelValidationResult
                {
                    Results = ModelState.ToDictionary(
                        m => m.Key,
                        m => string.Join('\n', m.Value.Errors.Select(e => e.ErrorMessage)))
                };

                return BadRequest(ValidationErrors);
            }

            _context.Entry(mapper.Map<Treatment>(treatmentDTO)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TreatmentExists(key))
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

        [HttpPost]
        [EnableQuery]
        public async Task<ActionResult<TreatmentDTO>> Post([FromBody] TreatmentDTO treatmentDTO)
        {
            if (!ModelState.IsValid)
            {
                var ValidationErrors = new ModelValidationResult
                {
                    Results = ModelState.ToDictionary(
                        m => m.Key,
                        m => string.Join('\n', m.Value.Errors.Select(e => e.ErrorMessage)))
                };

                return BadRequest(ValidationErrors);
            }

            Patient patient = await _context.Patients
                .Include(p => p.Appointments)
                .Where(p => p.Id == treatmentDTO.PatientId)
                .FirstOrDefaultAsync();

            if (patient == null)
            {
                return BadRequest(new ModelValidationResult { Results = new Dictionary<string, string>() { ["Patient.Id"] = "Patient not found." } });
            }

            var treatment = mapper.Map<Treatment>(treatmentDTO);

            patient.Treatments.Add(treatment);

            await _context.SaveChangesAsync();

            treatmentDTO.Id = treatment.Id;

            return CreatedAtAction(nameof(Get), new { key = treatmentDTO.Id }, treatmentDTO);
        }

        [HttpDelete]
        [EnableQuery]
        public async Task<IActionResult> Delete(int key)
        {
            var treatments = await _context.Treatments.FindAsync(key);
            if (treatments == null)
            {
                return NotFound();
            }

            _context.Treatments.Remove(treatments);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TreatmentExists(int id)
        {
            return _context.Treatments.Any(e => e.Id == id);
        }
    }
}
