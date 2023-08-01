
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OnlineCourse.Shared.Services;

namespace OnlineCourse.Services.Payment;

public static class ServiceRegistration
{
    public static void AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddHttpContextAccessor();
        services.AddScoped<ISharedIdentityService,SharedIdentityService>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.Authority = configuration["IdentityServerURL"];
            opt.Audience = "resource_payment";
            opt.RequireHttpsMetadata = false;
        });

        services.AddMassTransit(x =>
        {
            //default port : 5672
            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["RabbitMQUrl"],"/", host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });
            });
        });

    }
}

