using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ic.Identity
{
    /// <summary>
    /// One In-Memory Configuration for IdentityServer => Just for Demo Use
    /// </summary>
    public class InMemoryConfiguration
    {
        public static IConfiguration Configuration { get; set; }
        /// <summary>
        /// Define which APIs will use this IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("clientservice", "CAS Client Service"),
            };
        }

        /// <summary>
        /// Define which Apps will use thie IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                //ClientService项目使用密码方式授权
                new Client
                {
                    ClientId = "cas.sg.web.nb",
                    ClientName = "CAS NB System MPA Client",
                    ClientSecrets = new [] { new Secret("websecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    //RedirectUris = {Configuration["Clients:MvcClient:RedirectUri"]},
                    //PostLogoutRedirectUris = { Configuration["Clients:MvcClient:PostLogoutRedirectUri"] },//{ $"http://{Configuration["Clients:MvcClient:IP"]}:{Configuration["Clients:MvcClient:Port"]}/signout-callback-oidc" },
                    AllowedScopes = new [] { "clientservice",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile }
                },
                //Blog项目采用OpenIDConnect方式
                new Client
                {
                    ClientId = "cas.sg.web.implicit",
                    ClientName = "CAS NB System MPA Client",
                    ClientSecrets = new [] { new Secret("websecret".Sha256()) },
                    RequireConsent = false,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { "http://10.0.75.1:5200/signin-oidc" },//登录成功后返回的客户端地址
                    PostLogoutRedirectUris = { "http://10.0.75.1:5200 /signout-callback-oidc" },//注销登录后返回的客户端地址
                    AllowedScopes = new [] { "clientservice",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile },
                    AllowAccessTokensViaBrowser = true  //can return access_token to this client
                }
            };
        }

        /// <summary>
        /// Define which IdentityResources will use this IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}
