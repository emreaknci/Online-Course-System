using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace OnlineCourse.Services.PhotoStock;

    public static class ServiceRegistration
    {
        public static void AddPhotoStockServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = configuration["IdentityServerURL"];
                opt.Audience = "resource_photo_stock";
                opt.RequireHttpsMetadata = false;
            });
        }
    }

