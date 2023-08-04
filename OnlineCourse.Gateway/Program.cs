using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using OnlineCourse.Gateway.DelegateHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"configuration.{builder.Environment.EnvironmentName.ToLower()}.json");

builder.Services.AddHttpClient<TokenExchangeDelegateHandler>();
builder.Services.AddOcelot().AddDelegatingHandler<TokenExchangeDelegateHandler>();

builder.Services.AddAuthentication()
    .AddJwtBearer("GatewayAuthenticationScheme", options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_gateway";
    options.RequireHttpsMetadata = false;
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


await app.UseOcelot();

app.Run();
