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
                //new Client
                //{
                //    ClientId = "cas.sg.web.nb",
                //    ClientName = "CAS NB System MPA Client",
                //    ClientSecrets = new [] { new Secret("websecret".Sha256()) },
                //    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                //    AllowedScopes = new [] { "clientservice",
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile }
                //},
                //Blog项目采用OpenIDConnect方式
                new Client
                {
                    ClientId = Configuration["Clients:BlogClient:ClientId"],
                    ClientName = Configuration["Clients:BlogClient:ClientName"],
                    ClientSecrets = new [] { new Secret("websecret".Sha256()) },
                    RequireConsent = false,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { Configuration["Clients:BlogClient:RedirectUri"] },//登录成功后返回的客户端地址
                    PostLogoutRedirectUris = { Configuration["Clients:BlogClient:PostLogoutRedirectUri"] },//注销登录后返回的客户端地址

                    AllowedCorsOrigins = {"http://www.icyrene.cn","http://47.94.156.149:5200"},
                    AllowedScopes = new [] { Configuration["Clients:BlogClient:AllowedScope"],
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
