using OnlineCourse.Web.Models.Catalog;

namespace OnlineCourse.Web.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CourseViewModel>> GetAllCourseAsync();

        Task<List<CategoryViewModel>> GetAllCategoryAsync();

        Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId);

        Task<CourseViewModel> GetByCourseIdAsync(string courseId);

        Task<bool> CreateCourseAsync(CourseCreateInput input);

        Task<bool> UpdateCourseAsync(CourseUpdateInput input);

        Task<bool> DeleteCourseAsync(string courseId);
    }
}
