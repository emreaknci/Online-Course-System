using OnlineCourse.Services.Catalog.Dtos;
using OnlineCourse.Shared.Dtos;

namespace OnlineCourse.Services.Catalog.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();

        Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto dto);

        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
