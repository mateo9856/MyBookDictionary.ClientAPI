using FluentValidation;
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
        private readonly IValidator<LoginUser> _validatorLogin;
        private readonly IValidator<CreateUser> _validaatorRegister;

        public IdentityController(IIdentityService identity, IValidator<LoginUser> validatorLogin, IValidator<CreateUser> validatorRegister)
        {
            _identity = identity;
            _validatorLogin = validatorLogin;
            _validaatorRegister = validatorRegister;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Login([FromBody]LoginUser user)
        {
            var ValidLogin = _validatorLogin.Validate(user);

            if(ValidLogin.IsValid)
            {
                var GetUser = await _identity.LoginUser(user);

                //add method to check if val ismfa

                return GetUser.Item1 == "Failed" ? BadRequest() :
                    GetUser.Item2 == "Success" ? Ok(new { Status = GetUser.Item1, Token = GetUser.Item2 }) : Forbid();
            }

            return BadRequest(ValidLogin.Errors);

        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] CreateUser user)
        {
            //register but before validate
            var ValidRegister = _validaatorRegister.Validate(user);

            if(ValidRegister.IsValid)
            {
                //send confirm email
                var UserCreate = await _identity.CreateUser(user);
                if (UserCreate) _identity.SendCheckEmail(user.Email); //end method in service
                
            }

            return Ok();
        }

    }
}
