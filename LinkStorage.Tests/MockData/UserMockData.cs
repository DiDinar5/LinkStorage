using LinkStorage.DTO;
using LinkStorage.Models;
using LinkStorage.Repository.IRepository;

namespace LinkStorage.Tests.MockData
{
    public class UserMockData: IUserRepository
    {
        public static LoginResponceDTO LoginResponce { get; set; } = new LoginResponceDTO
        {
            User = new User()
            {
                Id = 1,
                Name = "Test1",
                Surname = "Testov1",
                Role = "user",
                Email = "thisEmail@a.com",
                Password = "password"
            },
            Token = "asdasdasd"
        };

        public bool IsUniqueUser(string email)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponceDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public Task<User> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
