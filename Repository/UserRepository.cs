using LinkStorage.DataBase;
using LinkStorage.DTO;
using LinkStorage.Models;
using LinkStorage.Repository.IRepository;
using LinkStorage.Safety;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LinkStorage.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbLinkStorageContext _context;
        private string secretKey;
        public UserRepository(DbLinkStorageContext context, IConfiguration configuration)
        {
            _context = context;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if(user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponceDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower()
            && u.Password == Hash.HashPassword(loginRequestDTO.Password));

            if (user == null)
            {
                return new LoginResponceDTO()
                {
                    Token = "",
                    User = null
                };
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponceDTO loginResponceDTO = new LoginResponceDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = user,
            };
            return loginResponceDTO;
        }

        public async Task<User> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            
            User user = new()
            {
                Name = registrationRequestDTO.Name,
                Surname = registrationRequestDTO.Surname,
                Email = registrationRequestDTO.Email,
                Password = Hash.HashPassword(registrationRequestDTO.Password),
                Role = registrationRequestDTO.Role
            };
        
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
