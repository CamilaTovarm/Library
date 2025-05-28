using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Library.Models;
using Library.Services;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _BookService;
        public BookController(IBookService BookService)
        {
            _BookService = BookService;
        }


        // GET: api/<BookController>
        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAllBook()
        {
            return Ok(await _BookService.GetAll());
        }

        // GET api/<BookController>/5
        [HttpGet("{idBook}")]
        public async Task<ActionResult<Book>> GetBook(int idBook)
        {
            var Book = await _BookService.GetBook(idBook);
            if (Book == null)
            {
                return BadRequest("Book not found :(");
            }
            return Ok(Book);
        }

        // Book: api/Book
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(string title, string isbn, DateOnly publicationDate, int pageCount, int editorialId, int countryId, string imgUrl, int authorId, bool loanState)
        {
            var BookToPut = await _BookService.CreateBook(title, isbn, publicationDate, pageCount, editorialId, countryId, imgUrl, authorId, loanState);

            if (BookToPut != null)
            {
                return Ok(BookToPut);
            }
            else
            {
                return BadRequest("Error when inserting into the database :(");
            }


        }

        // PUT: api/Book/5
        [HttpPut("Update/{idBook}")]
        public async Task<ActionResult<Book>> PutBook(int idBook, string title, string isbn, DateOnly publicationDate, int pageCount, int editorialId, int countryId, string imgUrl, int authorId,bool loanState)
        {
            var BookToPut = await _BookService.UpdateBook(idBook, title, isbn, publicationDate, pageCount, editorialId, countryId, imgUrl, authorId, loanState);

            if (BookToPut != null)
            {
                return Ok(BookToPut);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }

        }

        // Delete: api/Book/5
        [HttpDelete("Delete/{idBook}")]
        public async Task<ActionResult<Book>> DeleteBook(int idBook)
        {

            var BookToDelete = await _BookService.DeleteBook(idBook);

            if (BookToDelete != null)
            {
                return Ok(BookToDelete);
            }
            else
            {
                return BadRequest("Error updating the database :(");
            }
        }
    }

}