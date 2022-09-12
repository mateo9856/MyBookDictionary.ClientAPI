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
using MyBookDictionary.Infra.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;

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
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = Guid.NewGuid(),
                Password = CryptoHelper.EncodeToMD5(user.Password),
                DateOfBirth = DateTime.Now,
                Email = user.Email,
                UserType = UserType.Client,
                IsUsedMFA = user.UsedMFA
            };

            var NewRole = new UserRole
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Client",
                User = NewUser
            };



            await _context.Users.AddAsync(NewUser);
            await _context.UsersRoles.AddAsync(NewRole);
                try
                {
                    SendCheckEmail(NewUser.Email);
                    await _context.SaveChangesAsync();
                    return true;

                } catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }   
            return false;

        }

        public async Task<AccountUser> GetById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<(string, object)> LoginUser(LoginUser user)
        {
            var GetUser = await _context.Users.FirstOrDefaultAsync(d => d.Email == user.Email && d.Password == CryptoHelper.EncodeToMD5(user.Password));

            if (GetUser != null)
            {
                return ("Success", WriteToken(GetUser));
            }
            else if (GetUser != null && GetUser.IsUsedMFA)
            {
                return ("MFA", string.Format("User authenticated go to {0} and type MFA key to confirm your authentication.", GetUser.Email));
            }
            return ("Failed", "User not exist!");
        }

        public async Task RequestMFA(string email)
        {
            var rnd = new Random();

            var FindUser = await _context.Users.FirstOrDefaultAsync(d => d.Email == email && d.IsUsedMFA == true);

            string generatedCode = null;

            do
            {
                generatedCode = rnd.Next(99999).ToString();

            } while (_context.Users.Where(c => c.MFACode == generatedCode).Count() > 1);

            var from = "reply@bookDict.com";

            var message = new MailMessage(from, email);
            message.Subject = "MFA Authentication.";
            message.Body = "Hello.\n" +
                "Thank for your back!\n Your code is: " + generatedCode;

            SmtpClient smtpClient = new SmtpClient(email);

            smtpClient.UseDefaultCredentials = true;

            try
            {
                smtpClient.Send(message);
                FindUser.MFACode = generatedCode;
                _context.Users.Attach(FindUser);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SendCheckEmail(string email)
        {
            var from = "reply@bookDict.com";

            var message = new MailMessage(from, email);
            message.Subject = "Confirm your Email.";
            message.Body = "Hello.\n" +
                "Thank for a registering to Book Dictionary. Click below link to confirm register!\n" +
                "http://localhost:9412/api/confirm/653gdgerer";

            SmtpClient smtpClient = new SmtpClient(email);

            smtpClient.UseDefaultCredentials = true;
            
            try
            {
                smtpClient.Send(message);

            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<(string, string)> ConfirmMFA(string mfa)
        {
            var FindUser = await _context.Users.FirstOrDefaultAsync(d => d.MFACode == mfa);

            if(FindUser != null)
            {
                return ("Success", WriteToken(FindUser));
            }
            return ("Failed", "Wrong MFA!");
        }

        private string WriteToken(AccountUser user)
        {
            var claims = new[] {
                        new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, _options.Subject),
                        new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("Email", user.Email),
                        new Claim("Role", user.UserType.ToString())
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
