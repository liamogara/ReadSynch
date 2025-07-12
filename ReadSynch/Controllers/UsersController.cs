using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadSynch.Data;
using ReadSynch.Dtos;
using ReadSynch.Models;
using Microsoft.AspNetCore.Identity;

namespace ReadSynch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ReadSynchContext _context;
        private readonly PasswordHasher<string> _hasher;

        public UsersController(ReadSynchContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<string>();
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDto userDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
            {
                return BadRequest();
            }

            var passwordHash = _hasher.HashPassword(userDto.Username, userDto.Password);

            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = passwordHash,
                Name = userDto.Name
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
