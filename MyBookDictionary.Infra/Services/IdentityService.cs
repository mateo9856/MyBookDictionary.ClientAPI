using MyBookDictionary.Application.Requests.Identity;
using MyBookDictionary.Infra.Context.Identity;
using MyBookDictionary.Infra.Interfaces;
using MyBookDictionary.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBookDictionary.Infra.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Extensions.Options;
using MyBookDictionary.ClientAPI.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MyBookDictionary.Infra.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IdentityContext _context;
        private JwtOptions _options { get; }

        public IdentityService(IdentityContext context, IOptions<JwtOptions> options)
        {
            _context = context;
            _options = options.Value;
        }

        public async Task<bool> CreateUser(CreateUser user)
        {
            var NewUser = new AccountUser
            {
                Id = Guid.NewGuid(),
                Password = CryptoHelper.EncodeToMD5(user.Password),
                DateOfBirth = DateTime.Now,
                Email = user.Email,
                UserType = UserType.Client
            };

            var NewRole = new UserRole
            {
                RoleName = "Client",
                User = NewUser
            };

            await _context.Users.AddAsync(NewUser);
            await _context.UsersRoles.AddAsync(NewRole);
            int save = await _context.SaveChangesAsync();

            return save > 0 ? true : false;

        }

        public async Task<AccountUser> GetById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<object> LoginUser(LoginUser user)
        {
            var GetUser = await _context.Users.FirstOrDefaultAsync(d => d.Email == user.Email);

            if(GetUser != null)
            {
                var claims = new[] {
                        new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, _options.Subject),
                        new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", GetUser.Id.ToString()),
                        new Claim("Email", user.Email)
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _options.Issuer,
                    _options.Audience,
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);
            }

            return new
            {
                Status = "Failed",
                Error = "User doesn't exist!"
            };
        }

        public Task<bool> RequestMFA()
        {
            throw new NotImplementedException();
        }
    }
}
