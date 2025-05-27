using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Library.Models;
using Library.Services;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorialController : ControllerBase
    {
        private readonly IEditorialService _EditorialService;
        public EditorialController(IEditorialService EditorialService)
        {
            _EditorialService = EditorialService;
        }


        // GET: api/<EditorialController>
        [HttpGet]
        public async Task<ActionResult<List<Editorial>>> GetAllEditorial()
        {
            return Ok(await _EditorialService.GetAll());
        }

        // GET api/<EditorialController>/5
        [HttpGet("{idEditorial}")]
        public async Task<ActionResult<Editorial>> GetEditorial(int idEditorial)
        {
            var Editorial = await _EditorialService.GetEditorial(idEditorial);
            if (Editorial == null)
            {
                return BadRequest("Editorial not found :(");
            }
            return Ok(Editorial);
        }

        // Editorial: api/Editorial
        [HttpPost]
        public async Task<ActionResult<Editorial>> PostEditorial(string nameEditorial)
        {
            var EditorialToPut = await _EditorialService.CreateEditorial(nameEditorial);

            if (EditorialToPut != null)
            {
                return Ok(EditorialToPut);
            }
            else
            {
                return BadRequest("Error when inserting into the database :(");
            }


        }

        // PUT: api/Editorial/5
        [HttpPut("Update/{idEditorial}")]
        public async Task<ActionResult<Editorial>> PutEditorial(int idEditorial, string nameEditorial)
        {
            var EditorialToPut = await _EditorialService.UpdateEditorial(idEditorial, nameEditorial);

            if (EditorialToPut != null)
            {
                return Ok(EditorialToPut);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }

        }

        // Delete: api/Editorial/5
        [HttpDelete("Delete/{idEditorial}")]
        public async Task<ActionResult<Editorial>> DeleteEditorial(int idEditorial)
        {

            var EditorialToDelete = await _EditorialService.DeleteEditorial(idEditorial);

            if (EditorialToDelete != null)
            {
                return Ok(EditorialToDelete);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }
        }
    }

}