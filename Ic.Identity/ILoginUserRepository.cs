using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Identity
{
    public interface ILoginUserRepository : IRepository<LoginUser, IdentityDbContext>
    {
        LoginUser Authenticate(string _userName, string _userPassword);
    }
}
