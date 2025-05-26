using Library.Models;
using Library.Repositories;

namespace Library.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetAll();
        Task<Book> GetBook(int id);
        Task<Book> CreateBook(string title,string isbn, DateOnly publicationDate, int pageCount, int editorialId, int countryId);
        Task<Book> UpdateBook(int id, string? title = null, string? isbn=null, DateOnly? publicationDate = null, int? pageCount=null, int? editorialId=null, int countryId=null);
        Task<Book> DeleteBook(int id);
    }
    public class BookService: IBookService
    {
        private readonly IBooksRepository _booksRepository;
        public BookService(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }
        public Task<Book> CreateBook(string title, string isbn, DateOnly publicationDate, int pageCount, int editorialId, int countryId)
        {
            return _booksRepository.CreateBook(title, isbn, publicationDate, pageCount, editorialId, countryId);
        }
        public async Task<Book> DeleteBook(int id)
        {
            Book bookToDelete = await _booksRepository.GetBookById(id);
            if (bookToDelete == null)
            {
                throw new Exception($"This Book with the Id {id} doesn't exist.");
            }
            bookToDelete.State = true;
            bookToDelete.CreateDate = DateTime.Now;
            return await _booksRepository.DeleteBook(bookToDelete);
        }
        public async Task<List<Book>> GetAll()
        {
            return await _booksRepository.GetAllBooks();
        }
        public async Task<Book> GetBook(int id)
        {
            return await _booksRepository.GetBookById(id);
        }
        public async Task<Book> UpdateBook(int id, string? title = null, string? isbn = null, DateOnly? publicationDate = null, int? pageCount = null, int? editorialId = null, int? countryId=null)
        {
            Book bookToUpdate = await _booksRepository.GetBookById(id);
            if (bookToUpdate != null)
            {
                if (title != null)
                {
                    bookToUpdate.BookTitle = title;
                }
                if (isbn != null)
                {
                    bookToUpdate.ISBN = isbn;
                }
                if (publicationDate != null)
                {
                    bookToUpdate.PublicationDate = (DateOnly)publicationDate;
                }
                if (pageCount != null)
                {
                    bookToUpdate.PageCount = (int)pageCount;
                }
                if (editorialId != null)
                {
                    bookToUpdate.EditorialId = (int)editorialId;
                }
                if (countryId != null)
                {
                    bookToUpdate.CountryId = (int)countryId;
                }
                return await _booksRepository.UpdateBook(bookToUpdate);
            }
            throw new NotImplementedException();
        }
    }
}
