using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinkStorage.Models;
using Microsoft.AspNetCore.JsonPatch;
using LinkStorage.DTO;
using LinkStorage.DataBase;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;

namespace LinkStorage.Controllers
{
    [Route("api/users")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DbLinkStorageContext _context;
        protected OutputMessages output;

        public UsersController(DbLinkStorageContext context)
        {
            _context = context;
            this.output = new();
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
        /// <param name="email"></param>
        /// <param name="smartContractDto"></param>
        /// <returns></returns>
        [HttpPost("{email}/SmartContracts")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<User>> CreateSmartContract(string email, SmartContractDTO smartContractDto)
        {
            try
            {
                var user = await _context.Users
                .Include(x => x.SmartContracts)
                .FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

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

                return Created(user.Email, user.SmartContracts);
            }
            catch(ArgumentNullException a)
            {
                return BadRequest("Kukuebat");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Посмотреть отдельного пользователя(:admin)
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        // GET: api/Users/5
        [HttpGet("{email}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<User>> GetUser(string email)
        {
            var user = await _context.Users.Include(x => x.SmartContracts).FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
                return NotFound("The user does not exist");

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
        public async Task<ActionResult> UpdateUserPartial(uint id, [FromBody] JsonPatchDocument<User> patchDocument)
        {
            if (patchDocument == null || id == 0)
            {
                return BadRequest();
            }
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return NotFound("The user does not exist");
            var user = new User
            {
                Id = existingUser.Id,
                Name = existingUser.Name,
                Surname = existingUser.Surname,
                SmartContracts = existingUser.SmartContracts
            };
            patchDocument.ApplyTo(user, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existingUser.Name = user.Name;
            existingUser.Surname = user.Surname;
            existingUser.SmartContracts = user.SmartContracts;
            _context.SaveChanges();

            output.Messages.Add("User changed");
            return Ok(output);
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
                return NotFound("The user does not exist");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            output.Messages.Add("User deleted");
            return Ok(output);
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
                return NotFound("The smart-contract does not exist");

            _context.SmartContracts.Remove(link);
            await _context.SaveChangesAsync();

            output.Messages.Add("Smart - contract deleted");
            return Ok(output);
        }

    }
}
