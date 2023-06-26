using LinkStorage.DTO;
using LinkStorage.Models;


namespace LinkStorage.Tests.MockData
{
    public class UserMockData
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
    }
}
