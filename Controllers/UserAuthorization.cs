using LinkStorage.DataBase;
using LinkStorage.DTO;
using LinkStorage.Models;
using LinkStorage.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace LinkStorage.Controllers
{
    [Route("api/UserAuth")]
    [ApiController]
    public class UserAuthorization : Controller
    {
        private readonly IUserRepository _userRepository;
        protected APIResponse _response;
        public UserAuthorization(IUserRepository userRepository)
        {
                _userRepository= userRepository;
                this._response = new ();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepository.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess= false;
                _response.ErrorMessages.Add("Login or password is incorrect" );
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.SuccessMessage.Add("Welcome! Your unique token=>");
            _response.Result = loginResponse.Token;
            return Ok(_response);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            bool ifUserEmailUnique = _userRepository.IsUniqueUser(model.Email);
            if(ifUserEmailUnique!=null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Email already exists");
                return BadRequest(_response);
            }
            if (!new EmailAddressAttribute().IsValid(model.Email))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Incorrect mail format");
                return BadRequest(_response);
            }
            if (model.Password.Length < 8)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("The password must contain at least 8 characters");
                return BadRequest(_response);
            }
            var user =await  _userRepository.Register(model);   
            if(user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering");
                return NotFound(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.SuccessMessage.Add("Congratulations! The user is registered");
            return Ok(_response) ;
        }
    }
}
