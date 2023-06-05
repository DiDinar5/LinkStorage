using LinkStorage.DTO;
using LinkStorage.Models;

namespace LinkStorage.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string email);
        Task<LoginResponceDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<User> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
