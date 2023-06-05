using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinkStorage.Models;
using Microsoft.AspNetCore.JsonPatch;
using LinkStorage.DTO;
using LinkStorage.DataBase;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using System.Net;

namespace LinkStorage.Controllers
{
    [Route("api/users")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DbLinkStorageContext _context;
        protected APIResponse _response;

        public UsersController(DbLinkStorageContext context)
        {
            _context = context;
            this._response = new();
        }
        /// <summary>
        /// Посмотреть всех пользователей(:all)
        /// </summary>
        /// <returns></returns>
        // GET: api/Users
        [HttpGet("get")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = _context.Users
                .Select(x => new UserDTO
                {
                    Name = x.Name,
                    Surname = x.Surname,
                    Email = x.Email
                });
            if (!await _context.Users.AnyAsync())
            {
                return NotFound();
            }
            return await users.ToListAsync();
        }
        /// <summary>
        /// Добавить к существующему пользователю смарт-контракт(:admin)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="smartContractDto"></param>
        /// <returns></returns>
        [HttpPost("{userId}/SmartContracts")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<User>> CreateSmartContract(uint userId, SmartContractDTO smartContractDto)
        {
            var user = await _context.Users
                .Include(x => x.SmartContracts)
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("The user does not exist");
                return NotFound(_response);
            }

            var newContract = new SmartContract
            {
                LinkToContract = smartContractDto.LinkToContract,
                DateTimeCreated = DateTime.UtcNow
            };

            if (user.SmartContracts is null)
            {
                user.SmartContracts = new List<SmartContract>();
            }

            user.SmartContracts.Add(newContract);

            var updatedUser = _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return user;
        }
        /// <summary>
        /// Посмотреть отдельного пользователя(:admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<User>> GetUser(uint id)
        {
            var user = await _context.Users.Include(x => x.SmartContracts).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("The user does not exist");
                return NotFound(_response);
            }
            return user;
        }
        /// <summary>
        /// Изменение данных о пользователе(:admin)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        // POST: api/Users
        [HttpPatch("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdateUserPartial(uint id, [FromBody] JsonPatchDocument<User> patchDocument )
        {
            if (patchDocument == null || id==0)
            {
                return BadRequest();
            }
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("The user does not exist");
                return NotFound(_response);
            }
            var user = new User
            {
                Id = existingUser.Id,
                Name = existingUser.Name,
                Surname = existingUser.Surname,
                SmartContracts= existingUser.SmartContracts
            };
            patchDocument.ApplyTo(user,ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            existingUser.Name = user.Name;
            existingUser.Surname = user.Surname;
            existingUser.SmartContracts = user.SmartContracts;  
            _context.SaveChanges();

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.SuccessMessage.Add("User changed");
            return Ok(_response);
        }
        /// <summary>
        /// Удаление пользователя(:admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteUser(uint id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("The user does not exist");
                return NotFound(_response);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.SuccessMessage.Add("User deleted");
            return Ok(_response);
        }
        /// <summary>
        /// Удаление смарт-контракта у существующего пользователя(:admin)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        // DELETE: api/Users/5
        [HttpDelete("{Id}/SmartContract")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteLink(uint Id)
        {
            var link = await _context.SmartContracts.FindAsync(Id);
            if (link == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("The smart-contract does not exist");
                return NotFound(_response);
            }

            _context.SmartContracts.Remove(link);
            await _context.SaveChangesAsync();

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.SuccessMessage.Add("Smart-contract deleted");
            return Ok(_response);
        }

    }
}
