using Library.Models;
using Library.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Library.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<User> GetUser(int id);
        Task<User> CreateUser(string name, string UserName, string password, string email, int userTypeId);
        Task<User> UpdateUser (int id, string? name = null, string? email = null, string? UserName=null, string? password=null, int? userTypeId = null);
        Task<User> DeleteUser(int id);
        Task<bool> Authentication(string userName, string password);
        string GenerateToken(string username);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public Task<User> CreateUser(string name, string email, string UserName, string password, int userTypeId)
        {
            password = EncryptPassword(password);
            return _userRepository.CreateUser(name, UserName, password, email, userTypeId);
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
        public async Task<User> UpdateUser(int id, string? name = null, string? email = null, string? UserName = null, string? password = null, int? userTypeId = null)
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
                if (UserName != null)
                {
                    userToUpdate.UserName = UserName;
                }
                if (password != null)
                {
                    password = EncryptPassword(password);
                    userToUpdate.Password = password;
                }
                if (userTypeId != null)
                {
                    userToUpdate.UserTypeId = (int)userTypeId;
                }
                return await _userRepository.UpdateUser(userToUpdate);
            }
            throw new NotImplementedException();
        }



        // ENCRIPTAR CONTRASEÑA ----------------------------------------------------------------------------
        private string EncryptPassword(string password)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(password));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
        //AUTENTICACION----------------------------------------------------------------------------
        public async Task<bool> Authentication(string userName, string password)
        {
            var user = await _userRepository.AuthUser(userName);
            string hashedPassword = EncryptPassword(password);

            if (user != null && (user.Password == hashedPassword))
                return true;
            return false;
        }

        //TOKEN----------------------------------------------------------------------------
        public string GenerateToken(string username)
        {
            var key = _configuration.GetValue<string>("Jwt:key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, username));
            claims.AddClaim(new Claim(ClaimTypes.Role, "User"));

            var credencialesToken = new SigningCredentials
                (
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature
                );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = credencialesToken
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string tokenCreado = tokenHandler.WriteToken(tokenConfig);

            return tokenCreado;


        }
    }
}
