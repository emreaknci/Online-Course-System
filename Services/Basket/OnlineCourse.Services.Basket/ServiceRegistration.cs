using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using OnlineCourse.Services.Basket.Services;
using OnlineCourse.Services.Basket.Settings;
using OnlineCourse.Shared.Services;

namespace OnlineCourse.Services.Basket;

public static class ServiceRegistration
{
    public static void AddBasketServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
        services.AddSingleton<RedisService>(opt =>
        {
            var redisSettings = opt.GetRequiredService<IOptions<RedisSettings>>().Value;
            var redis = new RedisService(redisSettings.Host, redisSettings.Port);
            redis.Connect();
            return redis;
        });
        services.AddHttpContextAccessor();
        services.AddScoped<ISharedIdentityService, SharedIdentityService>();
        services.AddScoped<IBasketService, BasketService>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.Authority = configuration["IdentityServerURL"];
            opt.Audience = "resource_basket";
            opt.RequireHttpsMetadata = false;
        });

    }
}

