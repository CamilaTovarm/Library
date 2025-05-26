using Library.Context;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public interface IAuthorsRepository
    {
        Task<List<Authors>> GetAll();
        Task<Authors> GetAuthors(int id);
        Task<Authors> CreateAuthors(string AuthorsName);
        Task<Authors> UpdateAuthors(Authors authors);
        Task<Authors> DeleteAuthors(Authors authors);
    }
    public class AuthorRepository : IAuthorsRepository
    {
        private readonly LibraryDbContext _db;
        public AuthorRepository(LibraryDbContext db)
        {
            _db = db;
        }

        public async Task<Authors> CreateAuthors(string AuthorsName)
        {
            Authors newAuthors = new Authors
            {
                AuthorName = AuthorsName,
                CreateDate = null,
                State = false

            };
            await _db.Authors.AddAsync(newAuthors);
            await _db.SaveChangesAsync();
            return newAuthors;
        }

        public async Task<Authors> DeleteAuthors(Authors authors)
        {
            _db.Authors.Attach(authors);
            _db.Entry(authors).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return authors;
        }

        public async Task<Authors> GetAuthors(int id)
        {
            return await _db.Authors.FirstOrDefaultAsync(e => e.AuthorId == id);
        }

        public async Task<List<Authors>> GetAll()
        {
            return await _db.Authors.ToListAsync();
        }

        public async Task<Authors> UpdateAuthors(Authors authors)
        {
            Authors AuthorsUpdate = await _db.Authors.FindAsync(authors.AuthorId);
            if (AuthorsUpdate != null)
            {
                AuthorsUpdate.AuthorName = authors.AuthorName;
                await _db.SaveChangesAsync();
                return AuthorsUpdate;
            }
            return null;
        }
    }
}