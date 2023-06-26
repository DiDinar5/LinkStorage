using LinkStorage.DTO;
using LinkStorage.Models;
using LinkStorage.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LinkStorage.Controllers
{
    [Route("api/UserAuth")]
    [ApiController]
    public class UserAuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        protected OutputMessages output;
        public UserAuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            this.output = new();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepository.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
                return BadRequest("Login or password is incorrect");

            output.Messages.Add("Welcome! Your unique token=>");
            output.Result = loginResponse.Token;
            return Ok(output);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            bool ifUserEmailUnique = _userRepository.IsUniqueUser(model.Email);

            if (!ifUserEmailUnique)
                return BadRequest("Email already exists");

            if (!new EmailAddressAttribute().IsValid(model.Email))
                    return BadRequest("Incorrect mail format");

            if (model.Password.Length < 8)
                return BadRequest("The password must be at least 8 characters long ");
            
            var user = await _userRepository.Register(model);

            if (user == null)
                return NotFound("Error while registering");

            output.Messages.Add("Congratulations! The user is registered");
            output.Result = user.Email;
            return Ok(output);
        }
    }
}
