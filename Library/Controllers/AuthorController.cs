using Microsoft.AspNetCore.Http;
using Library.Models;
using Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _AuthorService;
        public AuthorController(IAuthorService AuthorService)
        {
            _AuthorService = AuthorService;
        }


        // GET: api/<UserTypeController>
        [HttpGet]
        public async Task<ActionResult<List<Authors>>> GetAllAuthor()
        {
            return Ok(await _AuthorService.GetAll());
        }

        // GET api/<UserTypeController>/5
        [HttpGet("{idAuthor}")]
        public async Task<ActionResult<Authors>> GetAuthors(int idAuthor)
        {
            var authors = await _AuthorService.GetAuthors(idAuthor);
            if (authors == null)
            {
                return BadRequest("Author not found :(");
            }
            return Ok(authors);
        }

        // UserType: api/UserType
        [HttpPost]
        public async Task<ActionResult<Authors>> PostAuthors(string authorName)
        {
            var AuthorToPut = await _AuthorService.CreateAuthors(authorName);

            if (AuthorToPut != null)
            {
                return Ok(AuthorToPut);
            }
            else
            {
                return BadRequest("Error when inserting into the database :(");
            }


        }

        // PUT: api/UserType/5
        [HttpPut("Update/{idAuthor}")]
        public async Task<ActionResult<Authors>> PutAuthors(int idAuthor, string authorName)
        {
            var AuthorToPut = await _AuthorService.UpdateAuthors(idAuthor, authorName);

            if (AuthorToPut != null)
            {
                return Ok(AuthorToPut);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }

        }

        // Delete: api/UserType/5
        [HttpDelete("Delete/{idAuthor}")]
        public async Task<ActionResult<Authors>> DeleteAuthor(int idAuthor)
        {

            var AuthorToDelete = await _AuthorService.DeleteAuthors(idAuthor);

            if (AuthorToDelete != null)
            {
                return Ok(AuthorToDelete);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }
        }
    }

}