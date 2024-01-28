using MedRoute.Database;
using MedRoute.Models.System;
using MedRoute.Repository;
using MedRoute.Repository.Implement;
using MedRoute.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddSession();

//Config routing format
services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

//Config region
services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "vi-VN" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

//Add enviroment variable
services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

//Setting DB
services.AddDbContext<AppDBContext>(options =>
{
    options.UseLazyLoadingProxies()
            .UseSqlServer(builder.Configuration.GetConnectionString("MSSQL"));
});

//Add services
services.AddScoped<IBookingRepository, BookingRepository>();
services.AddScoped<IRoleRepository, RoleRepository>();
services.AddSingleton<IHashPassword, HashPassword>();
services.AddScoped<IAuthenticateService, UserRepository>();
services.AddScoped<IUserRepository, UserRepository>();

//Setting Authentication
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Scheme";
    options.DefaultScheme = "Scheme";
})
.AddCookie("Scheme", options =>
{
    options.LoginPath = "/auth/login";
    options.AccessDeniedPath = "/";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.Cookie.MaxAge = options.ExpireTimeSpan;
    options.SlidingExpiration = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areaRoute",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
