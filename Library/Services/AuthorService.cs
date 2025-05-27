using Library.Models;
using Library.Repositories;

namespace Library.Services
{
    public interface IAuthorService
    {
        Task<List<Authors>> GetAll();
        Task<Authors> GetAuthors(int idAuthor);
        Task<Authors> CreateAuthors(string AuthorName);
        Task<Authors> UpdateAuthors(int idAuthor, string? AuthorName = null);
        Task<Authors> DeleteAuthors(int idAuthor);
    }
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorsRepository _authorsRepository;
        public AuthorService(IAuthorsRepository authorsRepository)
        {
            _authorsRepository = authorsRepository;
        }
        public async Task<Authors> CreateAuthors(string AuthorName)
        {
            return await _authorsRepository.CreateAuthors(AuthorName);
        }
        public async Task<Authors> DeleteAuthors(int idAuthor)
        {
            Authors AuthorsToDelete = await _authorsRepository.GetAuthors(idAuthor);
            if (AuthorsToDelete == null)
            {
                throw new Exception($"This Author with the Id {idAuthor} doesn't exist.");
            }
            AuthorsToDelete.State = true;
            AuthorsToDelete.CreateDate = DateTime.Now;
            return await _authorsRepository.DeleteAuthors(AuthorsToDelete);
        }
        public async Task<List<Authors>> GetAll()
        {
            return await _authorsRepository.GetAll();
        }
        public async Task<Authors> GetAuthors(int idAuthor)
        {
            return await _authorsRepository.GetAuthors(idAuthor);
        }
        public async Task<Authors> UpdateAuthors(int idAuthor, string? AuthorName = null)
        {
            Authors newAuthors = await _authorsRepository.GetAuthors(idAuthor);
            if (newAuthors != null)
            {
                if (AuthorName != null)
                {
                    newAuthors.AuthorName = (string)AuthorName;
                }
                return await _authorsRepository.UpdateAuthors(newAuthors);
            }
            throw new NotImplementedException();
        }
    }
}