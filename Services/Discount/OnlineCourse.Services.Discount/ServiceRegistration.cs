using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using OnlineCourse.Discount.Services;
using OnlineCourse.Shared.Services;

namespace OnlineCourse.Services.Discount;

public static class ServiceRegistration
{
    public static void AddDiscountServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.Authority = configuration["IdentityServerURL"];
            opt.Audience = "resource_discount";
            opt.RequireHttpsMetadata = false;
        });
        services.AddScoped<ISharedIdentityService, SharedIdentityService>();
        services.AddScoped<IDiscountService, DiscountService>();
    }
}

