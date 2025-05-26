using Library.Context;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUser();
        Task<User> GetUserById(int id);
        Task<User> CreateUser(string name, string email, int userTypeId);
        Task<User> UpdateUser(User user);
        Task<User> DeleteUser(User user);
    }
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _db;
        public UserRepository(LibraryDbContext db)
        {
            _db = db;
        }
        public async Task<User> CreateUser(string name, string email, int userTypeId)
        {
            UserType? userType = _db.UserType.FirstOrDefault(ut => ut.UserTypeId == userTypeId);
            

            User newUser = new User
            {
                Name = name,
                Email = email,
                UserTypeId = userTypeId,
                CreateDate = null,
                State = false
            };
            await _db.User.AddAsync(newUser);
            _db.SaveChanges();
            return newUser;
        }

        public async Task<User> DeleteUser(User user)
        {
            _db.User.Attach(user); //Llamamos la actualizacion
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllUser()
        {
            return await _db.User.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _db.User.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> UpdateUser(User user)
        {
            User userUpdate = await _db.User.FindAsync(user.UserId);

            if (userUpdate != null)
            {
                userUpdate.Name = user.Name;
                userUpdate.Email = user.Email;
                userUpdate.UserTypeId = user.UserTypeId;
                await _db.SaveChangesAsync();
            }

            return userUpdate;
        }
    }
}
