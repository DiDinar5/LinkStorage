using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinkStorage.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.AspNetCore.JsonPatch;
using LinkStorage.DTO;

namespace LinkStorage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DbLinkStorageContext _context;

        public UsersController(DbLinkStorageContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewDTO>>> GetUsers()
        {
            var users = _context.Authorization
                .Select(x => new UserViewDTO
                {
                    Name = x.UserInfo.Name,
                    Surname = x.UserInfo.Surname,
                    Email = x.Email
                });
            if (!await _context.Users.AnyAsync())
            {
                return NotFound();
            }
            return await users.ToListAsync();
        }
        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserSignupDTO userDTO)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DbLinkStorageContext.Users'  is null.");
            }
            if (_context.Authorization.Any(a => userDTO.Email == a.Email))
            {
                return BadRequest(new { Message = "Адрес почты уже занят" });
            }
            if (!new EmailAddressAttribute().IsValid(userDTO.Email))
            {
                return BadRequest(new { Message = "Неправильный формат почты" });
            }
            if (userDTO.Password.Length < 8)
            {
                return BadRequest(new { Message = "Пароль должен содержать не менее 8 символов" });
            }
            var auth = new Authorization()
            {
                Email = userDTO.Email,
                Password = userDTO.Password,
                UserInfo = new User()
                {
                    Name = userDTO.Name,
                    Surname = userDTO.Surname,
                }
            };
            _context.Authorization.Add(auth);
            await _context.SaveChangesAsync();

            var actionName = nameof(GetUser);
            var routeValues = new { id = auth.UserId };

            return CreatedAtAction(actionName, routeValues, auth.UserInfo);
        }

        [HttpPost("{userId}/SmartContracts")]
        public async Task<ActionResult<User>> CreateSmartContract(uint userId, SmartContractDTO smartContractDto)
        {
            var user = await _context.Users
                .Include(x => x.SmartContracts)
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var newContract = new SmartContract
            {
                LinkToContract = smartContractDto.LinkToContract,
                DateTimeCreated = smartContractDto.DateTimeCreated
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

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(uint id)
        {
            var user = await _context.Users.Include(x => x.SmartContracts).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // POST: api/Users
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateUserPartial(uint id, [FromBody] JsonPatchDocument<User> patchDocument )
        {
            if (patchDocument == null || id==0)
            {
                return BadRequest();
            }
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return NotFound();
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
           
            return Ok(new {Message = "User updated"});
        }
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(uint id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new {Message = "User deleted"});
        }

        private bool UserExists(uint id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
