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

namespace MyBookDictionary.Infra.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IdentityContext _context;

        public IdentityService(IdentityContext context)
        {
            _context = context;
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

        public Task<bool> LoginUser(LoginUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RequestMFA()
        {
            throw new NotImplementedException();
        }
    }
}
