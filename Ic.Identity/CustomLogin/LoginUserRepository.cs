using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Identity
{
    public class LoginUserRepository : RepositoryBase<LoginUser, IdentityDbContext>, ILoginUserRepository
    {
        public LoginUserRepository(IdentityDbContext dbContext) : base(dbContext)
        {
        }

        public LoginUser Authenticate(string _userName, string _userPassword)
        {
            var entity = DbContext.LoginUsers.FirstOrDefault(p => p.UserName == _userName && p.Password == _userPassword);
            return entity;
        }
    }
}
