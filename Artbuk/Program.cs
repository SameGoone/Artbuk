using Artbuk;
using Artbuk.Core.Interfaces;
using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ArtbukContext>(options => options.UseSqlServer(connection));

builder.Services.AddScoped<IPostRepository,
    EfPostRepository>();
builder.Services.AddScoped<IGenreRepository,
    EfGenreRepository>();
builder.Services.AddScoped<IPostInGenreRepository,
    EfPostInGenreRepository>();
builder.Services.AddScoped<IUserRepository,
    EfUserRepository>();
builder.Services.AddScoped<ISoftwareRepository,
    EfSoftwareRepository>();
builder.Services.AddScoped<IPostInSoftwareRepository,
    EfPostInSoftwareRepository>();
builder.Services.AddScoped<IRoleRepository,
    EfRoleRepository>();
builder.Services.AddScoped<ILikeRepository,
    EfLikeRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// аутентификация с помощью куки
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => { options.LoginPath = "/Profile/Login"; options.AccessDeniedPath = "/Home/AccessDenied"; });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Feed}/{action=Feed}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ArtbukContext>();
        SampleData.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();
