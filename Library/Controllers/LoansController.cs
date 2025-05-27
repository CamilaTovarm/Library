using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Library.Models;
using Library.Services;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoansService _LoansService;
        public LoansController(ILoansService LoansService)
        {
            _LoansService = LoansService;
        }


        // GET: api/<LoansController>
        [HttpGet]
        public async Task<ActionResult<List<Loans>>> GetAllLoans()
        {
            return Ok(await _LoansService.GetAll());
        }

        // GET api/<LoansController>/5
        [HttpGet("{idLoans}")]
        public async Task<ActionResult<Loans>> GetLoans(int idLoans)
        {
            var Loans = await _LoansService.GetLoan(idLoans);
            if (Loans == null)
            {
                return BadRequest("Loans not found :(");
            }
            return Ok(Loans);
        }

        // Loans: api/Loans
        [HttpPost]
        public async Task<ActionResult<Loans>> PostLoans(int userId, int bookId, DateTime loandDate)
        {
            var LoansToPut = await _LoansService.CreateLoan(userId, bookId, loandDate);

            if (LoansToPut != null)
            {
                return Ok(LoansToPut);
            }
            else
            {
                return BadRequest("Error when inserting into the database :(");
            }


        }

        // PUT: api/Loans/5
        [HttpPut("Update/{idLoans}")]
        public async Task<ActionResult<Loans>> PutLoans(int idLoans, int userId, int bookId, DateTime loandDate)
        {
            var LoansToPut = await _LoansService.UpdateLoan(idLoans, userId, bookId, loandDate);

            if (LoansToPut != null)
            {
                return Ok(LoansToPut);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }

        }

        // Delete: api/Loans/5
        [HttpDelete("Delete/{idLoans}")]
        public async Task<ActionResult<Loans>> DeleteLoans(int idLoans)
        {

            var LoansToDelete = await _LoansService.DeleteLoan(idLoans);

            if (LoansToDelete != null)
            {
                return Ok(LoansToDelete);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }
        }
    }

}