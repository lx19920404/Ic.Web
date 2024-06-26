﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Identity
{
    public class LoginUserService : ILoginUserService
    {
        private ILoginUserRepository loginUserRepository;

        public LoginUserService(ILoginUserRepository _loginUserRepository)
        {
            this.loginUserRepository = _loginUserRepository;
        }

        public bool Authenticate(string _userName, string _userPassword, out LoginUser loginUser)
        {
            // some business logic code here ...
            // eg.Security check & MD5 & 3DES ...
            loginUser = loginUserRepository.Authenticate(_userName, _userPassword);
            return loginUser == null ? false : true;
        }
    }
}
