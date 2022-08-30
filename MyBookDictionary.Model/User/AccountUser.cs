using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Model.User
{
    public class AccountUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserType UserType { get; set; }
        public DateTime DeleteDate { get; set; }

        public string FullDescription
        {
            get
            {
                return string.Format("Name: {0}, {1} \n Email: {2} \n Date of Birth: {3}", LastName, FirstName, Email, DateOfBirth.ToShortDateString());
            }
        }

    }
}
