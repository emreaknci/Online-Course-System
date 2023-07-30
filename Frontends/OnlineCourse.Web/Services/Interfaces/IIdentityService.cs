using IdentityModel.Client;
using OnlineCourse.Shared.Dtos;
using OnlineCourse.Web.Models;

namespace OnlineCourse.Web.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SignInInput input);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
    }
}
