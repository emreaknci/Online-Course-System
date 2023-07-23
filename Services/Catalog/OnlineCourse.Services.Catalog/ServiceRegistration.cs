using Microsoft.Extensions.Options;
using OnlineCourse.Services.Catalog.Services;
using OnlineCourse.Services.Catalog.Settings;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace OnlineCourse.Services.Catalog
{
    public static class ServiceRegistration
    {
        public static void AddCatalogServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = configuration["IdentityServerURL"];
                opt.Audience = "resource_catalog";
                opt.RequireHttpsMetadata = false;
            });
        }
    }
}
