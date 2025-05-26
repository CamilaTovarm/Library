using Library.Context;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public interface IEditorialRepository
    {
        Task<List<Editorial>> GetAll();
        Task<Editorial> GetEditorial(int id);
        Task<Editorial> CreateEditorial(string editorialName);
        Task<Editorial> UpdateEditorial(Editorial editorial);
        Task<Editorial> DeleteEditorial(Editorial editorial);
    }
    public class EditorialRepository: IEditorialRepository
    {
        private readonly LibraryDbContext _db;
        public EditorialRepository(LibraryDbContext db)
        {
            _db = db;
        }
        public async Task<Editorial> CreateEditorial(string editorialName)
        {
            Editorial newEditorial = new Editorial
            {
                EditorialName = editorialName,
                CreateDate = DateTime.Now,
                State = true
            };
            await _db.Editorial.AddAsync(newEditorial);
            await _db.SaveChangesAsync();
            return newEditorial;
        }
        public async Task<Editorial> DeleteEditorial(Editorial editorial)
        {
            _db.Editorial.Attach(editorial);
            _db.Entry(editorial).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return editorial;
        }
        public async Task<List<Editorial>> GetAll()
        {
            return await _db.Editorial.ToListAsync();
        }
        public async Task<Editorial> GetEditorial(int id)
        {
            return await _db.Editorial.FirstOrDefaultAsync(e => e.EditorialId == id);
        }
        public async Task<Editorial> UpdateEditorial(Editorial editorial)
        {
            Editorial editorialUpdate = await _db.Editorial.FindAsync(editorial.EditorialId);
            if (editorialUpdate != null)
            {
                editorialUpdate.EditorialName = editorial.EditorialName;
                await _db.SaveChangesAsync();
                return editorialUpdate;
            }
            return null;
        }
    }
}
