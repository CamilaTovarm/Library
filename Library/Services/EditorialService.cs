using Library.Models;
using Library.Repositories;

namespace Library.Services
{
    public interface IEditorialService
    {
        Task<List<Editorial>> GetAll();
        Task<Editorial> GetEditorial(int idEditorial);
        Task<Editorial> CreateEditorial(string EditorialName);
        Task<Editorial> UpdateEditorial(int idEditorial, string? EditorialName = null);
        Task<Editorial> DeleteEditorial(int idEditorial);
    }
    public class EditorialService: IEditorialService
    {
        private readonly IEditorialRepository _editorialRepository;
        public EditorialService(IEditorialRepository editorialRepository)
        {
            _editorialRepository = editorialRepository;
        }
        public async Task<Editorial> CreateEditorial(string editorialName)
        {
            return await _editorialRepository.CreateEditorial(editorialName);
        }
        public async Task<Editorial> DeleteEditorial(int idEditorial)
        {
            Editorial editorialToDelete = await _editorialRepository.GetEditorial(idEditorial);
            if (editorialToDelete == null)
            {
                throw new Exception($"This Editorial with the Id {idEditorial} doesn't exist.");
            }
            editorialToDelete.State = true;
            editorialToDelete.CreateDate = DateTime.Now;
            return await _editorialRepository.DeleteEditorial(editorialToDelete);
        }
        public async Task<List<Editorial>> GetAll()
        {
            return await _editorialRepository.GetAll();
        }
        public async Task<Editorial> GetEditorial(int idEditorial)
        {
            return await _editorialRepository.GetEditorial(idEditorial);
        }
        public async Task<Editorial> UpdateEditorial(int idEditorial, string? editorialName = null)
        {
            Editorial newEditorial = await _editorialRepository.GetEditorial(idEditorial);
            if (newEditorial != null)
            {
                if (editorialName != null)
                {
                    newEditorial.EditorialName = (string)editorialName;
                }
                return await _editorialRepository.UpdateEditorial(newEditorial);
            }
            throw new NotImplementedException();
        }
    }
}
