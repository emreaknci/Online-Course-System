
using System.Reflection;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Services.Order.Application.Consumers;
using OnlineCourse.Services.Order.Application.Handlers;
using OnlineCourse.Services.Order.Infrastructure;
using OnlineCourse.Shared.Services;

namespace OnlineCourse.Services.Order.API;

public static class ServiceRegistration
{
    public static void AddOrderServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), config =>
            {
                config.MigrationsAssembly("OnlineCourse.Services.Order.Infrastructure");
            });
        });

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommandHandler).Assembly));

        services.AddHttpContextAccessor();
        services.AddScoped<ISharedIdentityService, SharedIdentityService>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.Authority = configuration["IdentityServerURL"];
            opt.Audience = "resource_order";
            opt.RequireHttpsMetadata = false;
        });

        services.AddMassTransit(x =>
        {

            x.AddConsumer<CreateOrderMessageCommandConsumer>();
            x.AddConsumer<CourseNameChangedEventConsumer>();

            //default port : 5672
            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["RabbitMQUrl"], "/", host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });

                config.ReceiveEndpoint("create-order-service", e =>
                {
                    e.ConfigureConsumer<CreateOrderMessageCommandConsumer>(context);
                });

                config.ReceiveEndpoint("course-name-changed-event-order-service", e =>
                {
                    e.ConfigureConsumer<CourseNameChangedEventConsumer>(context);
                });
            });

        });
    }
}

