using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Identity
{
    public interface ILoginUserService
    {
        bool Authenticate(string _userName, string _userPassword, out LoginUser loginUser);
    }
}
