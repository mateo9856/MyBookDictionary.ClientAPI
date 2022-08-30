using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Model.User
{
    public class UserRole
    {
        public string RoleName { get; set; }
        public AccountUser User { get; set; }
    }
}
