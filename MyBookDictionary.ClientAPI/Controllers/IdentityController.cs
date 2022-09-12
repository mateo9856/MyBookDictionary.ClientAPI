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
        {//TODO: Email-service works email send.
            var ValidLogin = _validatorLogin.Validate(user);

            if(ValidLogin.IsValid)
            {
                var GetUser = await _identity.LoginUser(user);

                if(GetUser.Item1 == "MFA")
                {
                    _identity.RequestMFA(user.Email);
                    return Ok(new { Status = GetUser.Item1, Message = GetUser.Item2 });
                }

                return GetUser.Item1 == "Failed" ? BadRequest() :
                    GetUser.Item1 == "Success" ? Ok(new { Status = GetUser.Item1, Token = GetUser.Item2 }) : Forbid();
            }

            return BadRequest(ValidLogin.Errors);

        }

        [HttpPost("confirmMFA/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmMFA(string id)
        {
            var confirm = await _identity.ConfirmMFA(id);

            return confirm.Item1 == "Success" ? Ok(confirm) : BadRequest(confirm);
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
                try
                {
                    await _identity.CreateUser(user);
                    return Ok("Success! User created");

                } catch(Exception ex)
                {
                    throw ex;
                }
            }
            return BadRequest();

        }

    }
}
