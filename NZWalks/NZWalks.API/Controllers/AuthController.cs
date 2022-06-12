using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository repo;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository repo,ITokenHandler tokenHandler )
        {
            this.repo = repo;
            this.tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            var user = await repo.AuthenticateAsync(
                loginRequestDTO.Username, loginRequestDTO.Password);

            if (user != null)
            {
                //Generate a JWT Token
                var token = await tokenHandler.CreateTokenAsync(user);
                return Ok(token);
            }

            return BadRequest("Username or Password is incorrect.");
        }
    }
}
