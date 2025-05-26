using Microsoft.AspNetCore.Http;
using Library.Models;
using Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeeController : ControllerBase
    {
        private readonly IFeeService _FeeService;
        public FeeController(IFeeService FeeService)
        {
            _FeeService = FeeService;
        }


        // GET: api/<UserTypeController>
        [HttpGet]
        public async Task<ActionResult<List<Fee>>> GetAllFee()
        {
            return Ok(await _FeeService.GetAllFee());
        }

        // GET api/<UserTypeController>/5
        [HttpGet("{idFee}")]
        public async Task<ActionResult<Fee>> GetFeeById(int idFee)
        {
            var fee = await _FeeService.GetFeeById(idFee);
            if (fee == null)
            {
                return BadRequest("Fee not found :(");
            }
            return Ok(fee);
        }

        // UserType: api/UserType
        [HttpPost]
        public async Task<ActionResult<Fee>> PostFee(int daysMin, int daysMax, float feeValue, string feeDescription)
        {
            var FeeToPut = await _FeeService.CreateFee(daysMin, daysMax, feeValue, feeDescription);

            if (FeeToPut != null)
            {
                return Ok(FeeToPut);
            }
            else
            {
                return BadRequest("Error when inserting into the database :(");
            }


        }

        // PUT: api/UserType/5
        [HttpPut("Update/{idFee}")]
        public async Task<ActionResult<Fee>> PutFee(int idFee, int daysMin, int daysMax, float feeValue, string feeDescription)
        {
            var FeeToPut = await _FeeService.UpdateFee(idFee, daysMin, daysMax, feeValue, feeDescription);

            if (FeeToPut != null)
            {
                return Ok(FeeToPut);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }

        }

        // Delete: api/UserType/5
        [HttpDelete("Delete/{idFee}")]
        public async Task<ActionResult<Fee>> DeleteFee(int idFee)
        {

            var FeeToDelete = await _FeeService.DeleteFee(idFee);

            if (FeeToDelete != null)
            {
                return Ok(FeeToDelete);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }
        }
    }

}

