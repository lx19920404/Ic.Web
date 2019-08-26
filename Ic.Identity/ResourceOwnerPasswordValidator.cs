﻿using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ic.Identity
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private ILoginUserService loginUserService;

        public ResourceOwnerPasswordValidator(ILoginUserService _loginUserService)
        {
            this.loginUserService = _loginUserService;
        }
        //This method will be called when the request of "connect/token"
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            LoginUser loginUser = null;
            bool isAuthenticated = loginUserService.Authenticate(context.UserName, context.Password, out loginUser);
            if (!isAuthenticated)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid client credential");
            }
            else
            {
                context.Result = new GrantValidationResult(
                    subject: context.UserName,
                    authenticationMethod: "custom",
                    claims: new Claim[] {
                        new Claim("Name", context.UserName),
                        new Claim("Id", loginUser.Id.ToString()),
                        new Claim("RealName", loginUser.RealName),
                        new Claim("Email", loginUser.Email)
                    }
                );
            }

            return Task.CompletedTask;
        }
    }
}
