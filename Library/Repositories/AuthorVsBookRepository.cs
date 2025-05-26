using Library.Context;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public interface IAuthorVsBookRepository
    {
        Task<List<AuthorVsBook>> GetAll();
        Task<AuthorVsBook> GetAuthorVsBook(int id);
        Task<AuthorVsBook> CreateAuthorVsBook(int authorId, int bookId);
        Task<AuthorVsBook> UpdateAuthorVsBook(AuthorVsBook AuthorVsBook);
        Task<AuthorVsBook> DeleteAuthorVsBook(AuthorVsBook AuthorVsBook);
    }
    public class AuthorVsBookRepository
    {
        public readonly LibraryDbContext _db;
        public AuthorVsBookRepository(LibraryDbContext db)
        {
            _db = db;
        }
        public async Task<AuthorVsBook> CreateAuthorVsBook(int authorId, int bookId)
        {
            AuthorVsBook newAuthorVsBook = new AuthorVsBook
            {
                AuthorId = authorId,
                BookId = bookId,
                CreateDate = null,
                State = false
            };
            await _db.AuthorVsBooks.AddAsync(newAuthorVsBook);
            await _db.SaveChangesAsync();
            return newAuthorVsBook;
        }
        public async Task<AuthorVsBook> DeleteAuthorVsBook(AuthorVsBook AuthorVsBook)
        {
            _db.AuthorVsBooks.Attach(AuthorVsBook);
            _db.Entry(AuthorVsBook).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return AuthorVsBook;
        }
        public async Task<List<AuthorVsBook>> GetAll()
        {
            return await _db.AuthorVsBooks.ToListAsync();
        }
        public async Task<AuthorVsBook> GetAuthorVsBook(int id)
        {
            return await _db.AuthorVsBooks.FirstOrDefaultAsync(a => a.AuthorVsBookId == id);
        }
        public async Task<AuthorVsBook> UpdateAuthorVsBook(AuthorVsBook AuthorVsBook)
        {
            AuthorVsBook authorVsBookUpdate = await _db.AuthorVsBooks.FindAsync(AuthorVsBook.AuthorVsBookId);
            if (authorVsBookUpdate != null)
            {
                authorVsBookUpdate.AuthorId = AuthorVsBook.AuthorId;
                authorVsBookUpdate.BookId = AuthorVsBook.BookId;
                await _db.SaveChangesAsync();
                return authorVsBookUpdate;
            }
            return null;
        }
    }
}
