#nullable disable
using AutoMapper;
using AutoMapper.AspNet.OData;
using ClinicProject.Server.Data;
using ClinicProject.Server.Data.DBModels.PatientTypes;
using ClinicProject.Shared.DTOs;
using ClinicProject.Shared.Models.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ClinicProject.Server.Controllers
{
    public class PatientsController : ODataController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public PatientsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(ODataQueryOptions<PatientDTO> options)
        {
            var query = await _context.Patients.GetQueryAsync<PatientDTO, Patient>(mapper, options);

            return Ok(await query.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Get(int key, ODataQueryOptions<PatientDTO> options)
        {
            var result = await _context.Patients.Where(p => p.Id == key).GetQueryAsync(mapper, options);

            return Ok(await result.FirstOrDefaultAsync());
        }

        [HttpPut]
        [EnableQuery]
        public async Task<IActionResult> Put(int key, [FromBody] PatientDTO patientdto)
        {
            if (key != patientdto.Id)
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

            _context.Entry(mapper.Map<Patient>(patientdto)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(key))
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
        public async Task<IActionResult> Post([FromBody] PatientDTO patientDTO)
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

            var patient = mapper.Map<Patient>(patientDTO);

            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();

            patientDTO.Id = patient.Id;

            return CreatedAtAction(nameof(Get), new { key = patientDTO.Id }, patientDTO);
        }

        [HttpDelete]
        [EnableQuery]
        public async Task<IActionResult> Delete(int key)
        {
            var patient = await _context.Patients.FindAsync(key);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
