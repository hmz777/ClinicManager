#nullable disable
using AutoMapper;
using AutoMapper.AspNet.OData;
using ClinicProject.Server.Data;
using ClinicProject.Server.Data.DBModels.AppointmentsTypes;
using ClinicProject.Server.Data.DBModels.PatientTypes;
using ClinicProject.Shared.DTOs.Appointments;
using ClinicProject.Shared.Models.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ClinicProject.Server.Controllers
{
    public class AppointmentsController : ODataController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public AppointmentsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(ODataQueryOptions<AppointmentDTO> options)
        {
            var query = await _context.Appointments.GetQueryAsync<AppointmentDTO, Appointment>(mapper, options);
            return Ok(await query.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Get(int key, ODataQueryOptions<AppointmentDTO> options)
        {
            var result = await _context.Appointments.Where(p => p.Id == key).GetQueryAsync(mapper, options);
            return Ok(await result.FirstOrDefaultAsync());
        }

        [HttpPut]
        [EnableQuery]
        public async Task<IActionResult> Put(int key, [FromBody] AppointmentDTO appointmentDTO)
        {
            if (key != appointmentDTO.Id)
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

            _context.Entry(mapper.Map<Appointment>(appointmentDTO)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(key))
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
        public async Task<IActionResult> Post([FromBody] AppointmentDTO appointmentDTO)
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
                .Where(p => p.Id == appointmentDTO.Patient.Id)
                .FirstOrDefaultAsync();

            if (patient == null)
            {
                return BadRequest(new ModelValidationResult { Results = new Dictionary<string, string>() { ["Patient.Id"] = "Patient not found." } });
            }

            var appointment = mapper.Map<Appointment>(appointmentDTO);

            patient.Appointments.Add(appointment);

            await _context.SaveChangesAsync();

            appointmentDTO.Id = appointment.Id;

            return CreatedAtAction(nameof(Get), new { key = appointmentDTO.Id }, appointmentDTO);
        }

        [HttpDelete]
        [EnableQuery]
        public async Task<IActionResult> Delete(int key)
        {
            var appointment = await _context.Appointments.FindAsync(key);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
