using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Model.User
{
    public class RolesAttribute : Attribute
    {
        public string[] Roles { get; set; }

        public RolesAttribute(params string[] roles) => Roles = roles;

    }
}
