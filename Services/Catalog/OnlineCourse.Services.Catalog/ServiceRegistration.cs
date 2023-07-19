using Microsoft.Extensions.Options;
using OnlineCourse.Services.Catalog.Services;
using OnlineCourse.Services.Catalog.Settings;
using System.Reflection;

namespace OnlineCourse.Services.Catalog
{
    public static class ServiceRegistration
    {
        public static void AddCatalogServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
