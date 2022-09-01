using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBookDictionary.Application.Requests.Identity;
using MyBookDictionary.Infra.Interfaces;

namespace MyBookDictionary.ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identity;

        public IdentityController(IIdentityService identity)
        {
            _identity = identity;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginUser user)
        {
            //find user and return important value

            //if user used mfa request to mfa

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateUser user)
        {
            //register but before validate

            //send confirm email

            return Ok();
        }

    }
}
