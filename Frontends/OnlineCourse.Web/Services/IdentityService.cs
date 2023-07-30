using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OnlineCourse.Shared.Dtos;
using OnlineCourse.Web.Helpers;
using OnlineCourse.Web.Models;
using OnlineCourse.Web.Services.Interfaces;

namespace OnlineCourse.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient client, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = client;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var disco = await IdentityServiceHelper.GetDiscoveryDocumentAsync(_httpClient, _serviceApiSettings.IdentityBaseUri);
            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            var token = await IdentityServiceHelper.RequestRefreshTokenAsync(_httpClient, _clientSettings, refreshToken, disco.TokenEndpoint);

            if (token.IsError)
            {
                return null;
            }

            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();
            var properties = authenticationResult.Properties;
            properties.StoreTokens(IdentityServiceHelper.CreateAuthenticationProperties(token).GetTokens());

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

            return token;
        }

        public async Task RevokeRefreshToken()
        {
            var disco = await IdentityServiceHelper.GetDiscoveryDocumentAsync(_httpClient, _serviceApiSettings.IdentityBaseUri);
            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            var tokenRevocationRequest = await IdentityServiceHelper.CreateTokenRevocationRequest(_clientSettings, refreshToken, disco.RevocationEndpoint);
            await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
        }

        public async Task<Response<bool>> SignIn(SignInInput signinInput)
        {
            var discovery = await IdentityServiceHelper.GetDiscoveryDocumentAsync(_httpClient, _serviceApiSettings.IdentityBaseUri);
            var token = await IdentityServiceHelper.RequestPasswordTokenAsync(_httpClient, _clientSettings, signinInput.Email, signinInput.Password, discovery.TokenEndpoint);

            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();
                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Response<bool>.Fail(errorDto.Errors, 400);
            }

            var userInfo = await IdentityServiceHelper.GetUserInfoAsync(_httpClient,token.AccessToken, discovery.UserInfoEndpoint);

            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            ClaimsPrincipal claimsPrincipal = IdentityServiceHelper.BuildClaimsPrincipal(userInfo.Claims);
            var authenticationProperties = IdentityServiceHelper.CreateAuthenticationProperties(token);
            authenticationProperties.IsPersistent = signinInput.IsRemember;

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return Response<bool>.Success(200);
        }
    }
}



