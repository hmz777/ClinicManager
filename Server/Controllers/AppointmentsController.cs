#nullable disable
using AutoMapper;
using AutoMapper.AspNet.OData;
using ClinicProject.Server.Data;
using ClinicProject.Server.Data.DBModels.AppointmentsTypes;
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
    public class AppointmentsController : ODataController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public AppointmentsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<IActionResult> GetAppointments(ODataQueryOptions<AppointmentDTO> options)
        {
            var query = await _context.Appointments.GetQueryAsync<AppointmentDTO, Appointment>(mapper, options);

            return Ok(await query.ToListAsync());
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment([FromODataUri] int id, ODataQueryOptions<AppointmentDTO> options)
        {
            var result = await _context.Appointments.Where(p => p.Id == id).GetQueryAsync(mapper, options);
            return Ok(await result.FirstOrDefaultAsync());
        }

        // PUT: api/Appointments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableQuery]
        public async Task<IActionResult> PutAppointment([FromODataUri] int id, [FromODataBody] AppointmentDTO appointmentDTO)
        {
            if (!ModelState.IsValid || id != appointmentDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(mapper.Map<Appointment>(appointmentDTO)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        // POST: api/Appointments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        public async Task<ActionResult<Appointment>> PostAppointment([FromODataBody] AppointmentDTO appointmentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _context.Appointments.AddAsync(mapper.Map<Appointment>(appointmentDTO));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointment", new { id = appointmentDTO.Id }, appointmentDTO);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        [EnableQuery]
        public async Task<IActionResult> DeleteAppointment([FromODataUri] int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
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
