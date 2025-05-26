using Library.Models;
using Library.Repositories;

namespace Library.Services
{
    public interface IUserTypeService
    {
        Task<List<UserType>> GetAll();
        Task<UserType> GetUserType(int idUserType);
        Task<UserType> CreateUserType(string userTypeName);
        Task<UserType> UpdateUserType(int idUserType, string? userTypeName = null);
        Task<UserType> DeleteUserType(int idUserType);
    }
    public class UserTypeService
    {
        private readonly IUserTypeRepository _userTypeRepository;
        public UserTypeService(IUserTypeRepository userTypeRepository)
        {
            _userTypeRepository = userTypeRepository;
        }
        public async Task<UserType> CreateUserType(string nameUserType)
        {
            return await _userTypeRepository.CreateUserType(nameUserType);
        }

        public async Task<UserType> DeleteUserType(int idUserType)
        {
            UserType UserTypeToDelete = await _userTypeRepository.GetUserType(idUserType);
            if (UserTypeToDelete == null)
            {
                throw new Exception($"This UserType with the Id {idUserType} don´t exist. ");
            }
            UserTypeToDelete.State = true;
            UserTypeToDelete.CreateDate = DateTime.Now;
            return await _userTypeRepository.DeleteUserType(UserTypeToDelete);
        }

        public async Task<List<UserType>> GetAll()
        {
            return await _userTypeRepository.GetAll();
        }

        public async Task<UserType> GetUserType(int idUserType)
        {
            return await _userTypeRepository.GetUserType(idUserType);
        }

        public async Task<UserType> UpdateUserType(int idUserType, string? userTypeName = null)
        {
            UserType newUserType = await _userTypeRepository.GetUserType(idUserType);
            if (newUserType != null)
            {
                if (userTypeName != null)
                {
                    newUserType.UserTypeName = (string)userTypeName;
                }
                return await _userTypeRepository.UpdateUserType(newUserType);
            }
            throw new NotImplementedException();
        }
    }
}
