using Library.Context;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public interface IUserTypeRepository
    {
        Task<List<UserType>> GetAll();
        Task<UserType> GetUserType(int id);
        Task<UserType> CreateUserType(string UserTypeName);
        Task<UserType> UpdateUserType(UserType UserType);
        Task<UserType> DeleteUserType(UserType UserType);
    }
    public class UserTypeRepository : IUserTypeRepository
    {
        private readonly LibraryDbContext _db;
        public UserTypeRepository(LibraryDbContext db)
        {
            _db = db;
        }
        public async Task<UserType> CreateUserType(string UserTypeName)
        {
            UserType newUserType = new UserType
            {
                UserTypeName = UserTypeName,
                CreateDate = null,
                State = false
            };
            await _db.UserType.AddAsync(newUserType);
            _db.SaveChanges();
            return newUserType;
        }

        public async Task<UserType> DeleteUserType(UserType UserType)
        {
            _db.UserType.Attach(UserType); //Llamamos la actualizacion
            _db.Entry(UserType).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return UserType;
        }

        public async Task<List<UserType>> GetAll()
        {
            return await _db.UserType.ToListAsync();
        }

        public async Task<UserType> GetUserType(int id)
        {
            return await _db.UserType.FirstOrDefaultAsync(u => u.UserTypeId == id);
        }

        public async Task<UserType> UpdateUserType(UserType UserType)
        {
            UserType UserTypeUpdate = await _db.UserType.FindAsync(UserType.UserTypeId);
            if (UserTypeUpdate != null)
            {
                UserTypeUpdate.UserTypeName = UserType.UserTypeName;

                await _db.SaveChangesAsync();
            }
            return UserTypeUpdate;
        }
    }
}
