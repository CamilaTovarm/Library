using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Library.Models;
using Library.Services;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypeController : ControllerBase
    {
        private readonly IUserTypeService _UserTypeService;
        public UserTypeController(IUserTypeService UserTypeService)
        {
            _UserTypeService = UserTypeService;
        }


        // GET: api/<UserTypeController>
        [HttpGet]
        public async Task<ActionResult<List<UserType>>> GetAllUserType()
        {
            return Ok(await _UserTypeService.GetAll());
        }

        // GET api/<UserTypeController>/5
        [HttpGet("{idUserType}")]
        public async Task<ActionResult<UserType>> GetUserType(int idUserType)
        {
            var UserType = await _UserTypeService.GetUserType(idUserType);
            if (UserType == null)
            {
                return BadRequest("UserType not found :(");
            }
            return Ok(UserType);
        }

        // UserType: api/UserType
        [HttpPost]
        public async Task<ActionResult<UserType>> PostUserType(string nameUserType)
        {
            var UserTypeToPut = await _UserTypeService.CreateUserType(nameUserType);

            if (UserTypeToPut != null)
            {
                return Ok(UserTypeToPut);
            }
            else
            {
                return BadRequest("Error when inserting into the database :(");
            }


        }

        // PUT: api/UserType/5
        [HttpPut("Update/{idUserType}")]
        public async Task<ActionResult<UserType>> PutUserType(int idUserType, string nameUserType)
        {
            var UserTypeToPut = await _UserTypeService.UpdateUserType(idUserType, nameUserType);

            if (UserTypeToPut != null)
            {
                return Ok(UserTypeToPut);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }

        }

        // Delete: api/UserType/5
        [HttpDelete("Delete/{idUserType}")]
        public async Task<ActionResult<UserType>> DeleteUserType(int idUserType)
        {

            var UserTypeToDelete = await _UserTypeService.DeleteUserType(idUserType);

            if (UserTypeToDelete != null)
            {
                return Ok(UserTypeToDelete);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }
        }
    }

}