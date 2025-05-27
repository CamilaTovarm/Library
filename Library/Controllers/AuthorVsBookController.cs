using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Library.Models;
using Library.Services;
using System.Net;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorVsBookController : ControllerBase
    {
        private readonly IAuthorVsBookService _AuthorVsBookService;
        public AuthorVsBookController(IAuthorVsBookService AuthorVsBookService)
        {
            _AuthorVsBookService = AuthorVsBookService;
        }


        // GET: api/<AuthorVsBookController>
        [HttpGet]
        public async Task<ActionResult<List<AuthorVsBook>>> GetAllAuthorVsBook()
        {
            return Ok(await _AuthorVsBookService.GetAllAuthorVsBooks());
        }

        // GET api/<AuthorVsBookController>/5
        [HttpGet("{idAuthorVsBook}")]
        public async Task<ActionResult<AuthorVsBook>> GetAuthorVsBook(int idAuthorVsBook)
        {
            var AuthorVsBook = await _AuthorVsBookService.GetAuthorVsBook(idAuthorVsBook);
            if (AuthorVsBook == null)
            {
                return BadRequest("AuthorVsBook not found :(");
            }
            return Ok(AuthorVsBook);
        }

        // AuthorVsBook: api/AuthorVsBook
        [HttpPost]
        public async Task<ActionResult<AuthorVsBook>> PostAuthorVsBook(int authorId, int bookId)
        {
            var AuthorVsBookToPut = await _AuthorVsBookService.CreateAuthorVsBook( authorId,  bookId);

            if (AuthorVsBookToPut != null)
            {
                return Ok(AuthorVsBookToPut);
            }
            else
            {
                return BadRequest("Error when inserting into the database :(");
            }


        }

        // PUT: api/AuthorVsBook/5
        [HttpPut("Update/{idAuthorVsBook}")]
        public async Task<ActionResult<AuthorVsBook>> PutAuthorVsBook(int idAuthorVsBook, int authorId, int bookId)
        {
            var AuthorVsBookToPut = await _AuthorVsBookService.UpdateAuthorVsBook(idAuthorVsBook, authorId, bookId);

            if (AuthorVsBookToPut != null)
            {
                return Ok(AuthorVsBookToPut);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }

        }

        // Delete: api/AuthorVsBook/5
        [HttpDelete("Delete/{idAuthorVsBook}")]
        public async Task<ActionResult<AuthorVsBook>> DeleteAuthorVsBook(int idAuthorVsBook)
        {

            var AuthorVsBookToDelete = await _AuthorVsBookService.DeleteAuthorVsBook(idAuthorVsBook);

            if (AuthorVsBookToDelete != null)
            {
                return Ok(AuthorVsBookToDelete);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }
        }
    }

}