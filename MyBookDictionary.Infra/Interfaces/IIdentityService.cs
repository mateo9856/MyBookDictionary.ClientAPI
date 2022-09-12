using MyBookDictionary.Application.Requests.Identity;
using MyBookDictionary.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> CreateUser(CreateUser user);
        Task<AccountUser> GetById(Guid id);
        Task<(string, object)> LoginUser(LoginUser user);
        Task RequestMFA(string email);
        Task<(string, string)> ConfirmMFA(string mfa);
        void SendCheckEmail(string email);
    }
}
