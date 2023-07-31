using Microsoft.AspNetCore.Authentication.Cookies;
using OnlineCourse.Shared.Services;
using OnlineCourse.Web.Models;
using OnlineCourse.Web.Services;
using OnlineCourse.Web.Services.Interfaces;
using System.Configuration;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using OnlineCourse.Web;
using OnlineCourse.Web.Handler;
using OnlineCourse.Web.Helpers;
using OnlineCourse.Web.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));
builder.Services.Configure<ServiceApiSettings>(builder.Configuration.GetSection("ServiceApiSettings"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ClientCredentialTokenHandler>();
builder.Services.AddScoped<ResourceOwnerPasswordTokenHandler>();
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddSingleton<PhotoHelper>();

builder.Services.AddHttpClientServices(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining(typeof(CourseCreateInputValidator));
builder.Services.AddAccessTokenManagement();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
    opt =>
    {
        opt.LoginPath = "/Auth/SignIn";
        opt.ExpireTimeSpan = TimeSpan.FromDays(60);
        opt.SlidingExpiration = true;
        opt.Cookie.Name = "onlinecoursecookie";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
