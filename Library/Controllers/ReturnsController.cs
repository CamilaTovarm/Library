using Microsoft.AspNetCore.Http;
using Library.Models;
using Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnsController : ControllerBase
    {
        private readonly IReturnsService _ReturnsService;
        public ReturnsController(IReturnsService ReturnsService)
        {
            _ReturnsService = ReturnsService;
        }


        // GET: api/<UserTypeController>
        [HttpGet]
        public async Task<ActionResult<List<Returns>>> GetAll()
        {
            return Ok(await _ReturnsService.GetAll());
        }

        // GET api/<UserTypeController>/5
        [HttpGet("{idReturn}")]
        public async Task<ActionResult<Returns>> GetReturns(int idReturn)
        {
            var retunrs = await _ReturnsService.GetReturns(idReturn);
            if (retunrs == null)
            {
                return BadRequest("Return not found :(");
            }
            return Ok(retunrs);
        }

        // UserType: api/UserType
        [HttpPost]
        public async Task<ActionResult<Returns>> PostReturns(DateTime returnDate, int loanId, float fineImposed)
        {
            var ReturnsToPut = await _ReturnsService.CreateReturns(returnDate, loanId, fineImposed);

            if (ReturnsToPut != null)
            {
                return Ok(ReturnsToPut);
            }
            else
            {
                return BadRequest("Error when inserting into the database :(");
            }


        }

        // PUT: api/UserType/5
        [HttpPut("Update/{idReturn}")]
        public async Task<ActionResult<Returns>> PutReturns(int idReturn, DateTime returnDate, int loanId, float fineImposed)
        {
            var ReturnsToPut = await _ReturnsService.UpdateReturns(idReturn, returnDate, loanId, fineImposed);

            if (ReturnsToPut != null)
            {
                return Ok(ReturnsToPut);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }

        }

        // Delete: api/UserType/5
        [HttpDelete("Delete/{idReturn}")]
        public async Task<ActionResult<Returns>> DeleteReturns(int idReturn)
        {

            var ReturnsToDelete = await _ReturnsService.DeleteReturns(idReturn);

            if (ReturnsToDelete != null)
            {
                return Ok(ReturnsToDelete);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }
        }
    }

}

