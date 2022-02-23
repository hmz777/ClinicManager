#nullable disable
using AutoMapper;
using AutoMapper.AspNet.OData;
using ClinicProject.Server.Data;
using ClinicProject.Server.Data.DBModels.PaymentTypes;
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
    public class PaymentsController : ODataController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public PaymentsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<IActionResult> GetPayments(ODataQueryOptions<PaymentDTO> options)
        {
            var query = await _context.Payments.GetQueryAsync<PaymentDTO, Payment>(mapper, options);
            return Ok(await query.ToListAsync());
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment([FromODataUri] int id, ODataQueryOptions<PaymentDTO> options)
        {
            var result = await _context.Payments.Where(p => p.Id == id).GetQueryAsync(mapper, options);
            return Ok(await result.FirstOrDefaultAsync());
        }

        // PUT: api/Payments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableQuery]
        public async Task<IActionResult> PutPayment([FromODataUri] int id, [FromODataBody] PaymentDTO paymentDTO)
        {
            if (!ModelState.IsValid || id != paymentDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(mapper.Map<Payment>(paymentDTO)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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

        // POST: api/Payments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        public async Task<ActionResult<Payment>> PostPayment([FromODataBody] PaymentDTO paymentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _context.Payments.AddAsync(mapper.Map<Payment>(paymentDTO));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = paymentDTO.Id }, paymentDTO);
        }

        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        [EnableQuery]
        public async Task<IActionResult> DeletePayment([FromODataUri] int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}
