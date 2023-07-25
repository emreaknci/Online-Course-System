using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace OnlineCourse.IdentityServer;

public static class Config
{

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new("resource_catalog") { Scopes = { "catalog_full_permission" } },
            new("resource_photo_stock") { Scopes = { "photo_stock_full_permission" } },
            new("resource_basket") { Scopes = { "basket_full_permission" } },
            new(IdentityServerConstants.LocalApi.ScopeName)
        };
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.Email(),
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name =  "roles",
                DisplayName = "Roles",
                Description = "Kullanıcı rolleri",
                UserClaims = new[]{"role"}
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
         new ("catalog_full_permission","CatalogAPI için full erişim"),
         new ("photo_stock_full_permission","PhotoStockAPI için full erişim"),
         new ("basket_full_permission","BasketAPI için full erişim"),
         new (IdentityServerConstants.LocalApi.ScopeName,"IdentityServerAPI için full erişim"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new()
            {
                ClientName = "Asp.Net Core MVC",
                ClientId = "WebMVCClient",
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes =
                {
                    "catalog_full_permission" ,
                    "photo_stock_full_permission" ,
                    IdentityServerConstants.LocalApi.ScopeName
                }
            },
            new()
            {
                ClientName = "Asp.Net Core MVC",
                ClientId = "WebMVCClientForUser",
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Address,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.LocalApi.ScopeName,
                    "roles",                    
                    "basket_full_permission" ,
                },
                AllowOfflineAccess = true,
                AccessTokenLifetime = 3600,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                RefreshTokenUsage = TokenUsage.ReUse
            }
        };
}
