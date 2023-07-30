using OnlineCourse.Web.Models;

namespace OnlineCourse.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();

    }
}
