using FluentAssertions;
using LinkStorage.Controllers;
using LinkStorage.DTO;
using LinkStorage.Repository.IRepository;
using LinkStorage.Tests.MockData;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LinkStorage.Tests.API.Controllers
{
    public class TestUserAuthController
    {
        private LoginRequestDTO model;
        private LoginRequestDTO loginRequestDTO;

        [Fact]
        public async Task GetAllAsync_ShouldReturn200Status()
        {
            ///Arrange
            var _userRepos = new Mock<IUserRepository>();
            _userRepos.Setup(x => x.Login(loginRequestDTO)).ReturnsAsync(UserMockData.LoginResponce);

            var sut = new UserAuthController(_userRepos.Object);
            ///act
            var result = (OkObjectResult)await sut.Login(model);

            ///Assert
            result.StatusCode.Should().Be(200);
            _userRepos.Invocations.Should().HaveCount(1);
        }
    }
}
