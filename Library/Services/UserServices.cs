using Library.Models;
using Library.Repositories;

namespace Library.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<User> GetUser(int id);
        Task<User> CreateUser(string name, string email, int userTypeId);
        Task<User> UpdateUser (int id, string? name = null, string? email = null, int? userTypeId = null);
        Task<User> DeleteUser(int id);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<User> CreateUser(string name, string email, int userTypeId)
        {
            return _userRepository.CreateUser(name, email, userTypeId);
        }
        public async Task<User> DeleteUser(int id)
        {
            User userToDelete = await _userRepository.GetUserById(id);
            if (userToDelete == null)
            {
                throw new Exception($"This User with the Id {id} doesn't exist.");
            }
            userToDelete.State = true;
            userToDelete.CreateDate = DateTime.Now;
            return await _userRepository.DeleteUser(userToDelete);
        }
        public async Task<List<User>> GetAll()
        {
            return await _userRepository.GetAllUser();
        }
        public async Task<User> GetUser(int id)
        {
            return await _userRepository.GetUserById(id);
        }
        public async Task<User> UpdateUser(int id, string? name = null, string? email = null, int? userTypeId = null)
        {
            User userToUpdate = await _userRepository.GetUserById(id);
            if (userToUpdate != null)
            {
                if (name != null)
                {
                    userToUpdate.Name= name;
                }
                if (email != null)
                {
                    userToUpdate.Email = email;
                }
                if (userTypeId != null)
                {
                    userToUpdate.UserTypeId = (int)userTypeId;
                }
                return await _userRepository.UpdateUser(userToUpdate);
            }
            throw new NotImplementedException();
        }
    }
}
