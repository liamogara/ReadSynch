using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ReadSynch.Data;
using ReadSynch.Dtos;
using ReadSynch.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ReadSynch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ReadSynchContext _context;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<string> _hasher;

        public AuthController(ReadSynchContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _hasher = new PasswordHasher<string>();
        }
        
        // POST: api/Auth
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            if (!await _context.Users.AnyAsync(u => u.Username == loginDto.Username))
            {
                return Unauthorized();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (_hasher.VerifyHashedPassword(user.Username, user.PasswordHash, loginDto.Password) ==
                PasswordVerificationResult.Failed)
                return Unauthorized();
            else
            {
                var token = GenerateJwtToken(user, loginDto.RememberMe);
                return Ok(new { token });
            }
        }

        private string GenerateJwtToken(User user, bool rememberMe)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddMinutes(30);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
