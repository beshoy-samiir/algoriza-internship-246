using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Users
{
    public class Role : IdentityRole<int>
    {
        public Role() { }
        public Role(string name) : base(name)
        {

        }
    }
}
