using Library.Context;
using Library.Models;
using Library.Repositories;

namespace Library.Services
{
    public interface IAuthorVsBookService
    {
        Task<List<AuthorVsBook>> GetAllAuthorVsBooks();
        Task<AuthorVsBook> GetAuthorVsBook(int id);
        Task<AuthorVsBook> CreateAuthorVsBook(int authorId, int bookId);
        Task<AuthorVsBook> UpdateAuthorVsBook(int id, int? authorId=null, int? bookId=null);
        Task<AuthorVsBook> DeleteAuthorVsBook(int id);
    }
    public class AuthorVsBookService: IAuthorVsBookService
    {
        private readonly IAuthorVsBookRepository _authorVsBookRepository;
        public AuthorVsBookService(IAuthorVsBookRepository authorVsBookRepository)
        {
            _authorVsBookRepository = authorVsBookRepository;
        }
        public async Task<AuthorVsBook> CreateAuthorVsBook(int authorId, int bookId)
        {
            return await _authorVsBookRepository.CreateAuthorVsBook(authorId, bookId);
        }
        public async Task<AuthorVsBook> DeleteAuthorVsBook(int id)
        {
            AuthorVsBook authorVsBookToDelete = await _authorVsBookRepository.GetAuthorVsBook(id);
            if (authorVsBookToDelete == null)
            {
                throw new Exception($"This AuthorVsBook with the Id {id} doesn't exist.");
            }
            authorVsBookToDelete.State = true;
            authorVsBookToDelete.CreateDate = DateTime.Now;
            return await _authorVsBookRepository.DeleteAuthorVsBook(authorVsBookToDelete);
        }
        public async Task<List<AuthorVsBook>> GetAllAuthorVsBooks()
        {
            return await _authorVsBookRepository.GetAll();
        }
        public async Task<AuthorVsBook> GetAuthorVsBook(int id)
        {
            return await _authorVsBookRepository.GetAuthorVsBook(id);
        }
        public async Task<AuthorVsBook> UpdateAuthorVsBook(int id, int? authorId = null, int? bookId = null)
        {
            AuthorVsBook authorVsBookToUpdate = await _authorVsBookRepository.GetAuthorVsBook(id);
            if (authorVsBookToUpdate != null)
            {
                if (authorId != null)
                {
                    authorVsBookToUpdate.AuthorId = (int)authorId;
                }
                if (bookId != null)
                {
                    authorVsBookToUpdate.BookId = (int)bookId;
                }
                return await _authorVsBookRepository.UpdateAuthorVsBook(authorVsBookToUpdate);
            }
            throw new NotImplementedException();
        }

    }
}
