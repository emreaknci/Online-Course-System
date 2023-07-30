using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OnlineCourse.Shared.Dtos;
using OnlineCourse.Web.Models;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace OnlineCourse.Web.Helpers
{
    public static class IdentityServiceHelper
    {
        public static async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(HttpClient httpClient, string identityBaseUri)
        {
            var discoveryDocumentRequest = new DiscoveryDocumentRequest
            {
                Address = identityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            };

            var discovery = await httpClient.GetDiscoveryDocumentAsync(discoveryDocumentRequest);

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            return discovery;
        }

        public static async Task<TokenResponse> RequestRefreshTokenAsync(HttpClient httpClient, ClientSettings clientSettings, string refreshToken, string tokenEndpoint)
        {
            var refreshTokenRequest = new RefreshTokenRequest
            {
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = tokenEndpoint
            };

            return await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);
        }

        public static async Task<TokenRevocationRequest> CreateTokenRevocationRequest(ClientSettings clientSettings, string refreshToken, string revocationEndpoint)
        {
            var tokenRevocationRequest = new TokenRevocationRequest
            {
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                Address = revocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = OpenIdConnectParameterNames.RefreshToken
            };

            return tokenRevocationRequest;
        }

        public static async Task<TokenResponse> RequestPasswordTokenAsync(HttpClient httpClient, ClientSettings clientSettings, string email, string password, string tokenEndpoint)
        {
            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                UserName = email,
                Password = password,
                Address = tokenEndpoint
            };

            return await httpClient.RequestPasswordTokenAsync(passwordTokenRequest);
        }

        public static async Task<UserInfoResponse> GetUserInfoAsync(HttpClient httpClient, string accessToken, string userInfoEndpoint)
        {
            var userInfoRequest = new UserInfoRequest
            {
                Token = accessToken,
                Address = userInfoEndpoint
            };

            return await httpClient.GetUserInfoAsync(userInfoRequest);
        }

        public static ClaimsPrincipal BuildClaimsPrincipal(IEnumerable<Claim> claims)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            return new ClaimsPrincipal(claimsIdentity);
        }

        public static AuthenticationProperties CreateAuthenticationProperties(TokenResponse token)
        {
            var authenticationTokens = new List<AuthenticationToken>()
        {
            new() { Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken },
            new() { Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
            new() { Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture) }
        };

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(authenticationTokens);

            return authenticationProperties;
        }
    }
}
