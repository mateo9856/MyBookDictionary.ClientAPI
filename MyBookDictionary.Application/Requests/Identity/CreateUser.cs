using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Application.Requests.Identity
{
    public class CreateUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RetypePassword { get; set; }
        public bool UsedMFA { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
