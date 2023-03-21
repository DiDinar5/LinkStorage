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
            var users = from u in _context.Users
                        select new UserViewDTO()
                        {
                            Name = u.Name,
                            Surname = u.Surname,
                            Email = u.Authorization /*Email*/
                        };
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await users.ToListAsync();
        //    return await _context.Users.Include(u=>u.Authorization).Select(u=> new { name=u.Name, login=u.Authorization.Email}).ToListAsync();
        }
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
            User user = new User()
            {
                Name = userDTO.Name,
                Surname = userDTO.Surname,
                Authorization = new Authorization()
                {
                    Email = userDTO.Email, Password = userDTO.Password
                }
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user.Id);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(uint id)
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

            return user;
        }
        [HttpPatch("{id}")]
      //  [Route("{id}/UpdatePartial")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest )]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  ActionResult UpdateUserPartial(uint id, [FromBody] JsonPatchDocument<User> patchDocument )
        {
            if (patchDocument == null || id<=0)
            {
                return BadRequest();
            }
            var existingUser = _context.Users.Where(s=>s.Id==id).FirstOrDefault();
            if (existingUser == null)
                return NotFound();
            var user = new User
            {
                Id = existingUser.Id,
                Name = existingUser.Name,
                Surname = existingUser.Surname,
                Authorization= existingUser.Authorization,
                SmartContracts= existingUser.SmartContracts
            };
            patchDocument.ApplyTo(user,ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            existingUser.Name = user.Name;
            existingUser.Surname = user.Surname;
            existingUser.Authorization = user.Authorization;
            existingUser.SmartContracts = user.SmartContracts;  
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!UserExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return NoContent();
        }
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(uint id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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

            return NoContent();
        }

        private bool UserExists(uint id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
