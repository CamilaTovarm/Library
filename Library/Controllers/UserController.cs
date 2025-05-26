using Microsoft.AspNetCore.Http;
using Library.Models;
using Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;
        public UserController(IUserService UserService)
        {
            _UserService = UserService;
        }


        // GET: api/<UserTypeController>
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            return Ok(await _UserService.GetAll());
        }

        // GET api/<UserTypeController>/5
        [HttpGet("{idUser}")]
        public async Task<ActionResult<User>> GetUser(int idUser)
        {
            var user = await _UserService.GetUser(idUser);
            if (user == null)
            {
                return BadRequest("User not found :(");
            }
            return Ok(user);
        }

        // UserType: api/UserType
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(string name, string email, int userTypeId)
        {
            var UserToPut = await _UserService.CreateUser(name, email, userTypeId);

            if (UserToPut != null)
            {
                return Ok(UserToPut);
            }
            else
            {
                return BadRequest("Error when inserting into the database :(");
            }


        }

        // PUT: api/UserType/5
        [HttpPut("Update/{idUser}")]
        public async Task<ActionResult<User>> PutUser(int idUser, string name, string email, int userTypeId)
        {
            var UserToPut = await _UserService.UpdateUser(idUser,name, email, userTypeId);

            if (UserToPut != null)
            {
                return Ok(UserToPut);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }

        }

        // Delete: api/UserType/5
        [HttpDelete("Delete/{idUser}")]
        public async Task<ActionResult<User>> DeleteUser(int idUser)
        {

            var UserToDelete = await _UserService.DeleteUser(idUser);

            if (UserToDelete != null)
            {
                return Ok(UserToDelete);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }
        }
    }

}

