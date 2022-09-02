using FluentValidation;
using MyBookDictionary.Application.Requests.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Application.Validations.Identity
{
    public class LoginUserValidator : AbstractValidator<LoginUser>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);
            RuleFor(x => x.Email)
                .Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        }
    }
}
