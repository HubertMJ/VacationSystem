using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VacationSystem.Data;
using VacationSystem.Dtos;
using VacationSystem.Models;

namespace VacationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        
        public AuthController (IAuthRepository repo, IConfiguration config){
            _repo = repo;
            _config = config;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]WorkerForRegisterDto workerForRegisterDto)
        {
            workerForRegisterDto.Username = workerForRegisterDto.Username.ToLower();
            if (workerForRegisterDto.VacationDays == null)
                workerForRegisterDto.VacationDays = 0;
            if(workerForRegisterDto.PermissionLevel == null)
                workerForRegisterDto.PermissionLevel = 0;
                    
            if(await _repo.WorkerExists(workerForRegisterDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = new Worker{
                Username = workerForRegisterDto.Username,
                Name = workerForRegisterDto.Name,
                LastName = workerForRegisterDto.LastName,
                VacationDays = 0,
                PermissionLevel = 0
                //VacationDays = workerForRegisterDto.VacationDays,
                //PermissionLevel = workerForRegisterDto.PermissionLevel
            };

            var createdUser = await _repo.Register(userToCreate, workerForRegisterDto.Password);

            return StatusCode(201);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]WorkerForLoginDto workerForLoginDto){

            var workerFromRepo = await _repo.Login(workerForLoginDto.Username.ToLower(), workerForLoginDto.Password);

            if(workerFromRepo == null)
                return Unauthorized();

            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, workerFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, workerFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}