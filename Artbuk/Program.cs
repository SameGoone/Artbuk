using Artbuk;
using Artbuk.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ArtbukContext>(options => options.UseSqlServer(connection));

builder.Services.AddScoped<PostRepository>();
builder.Services.AddScoped<GenreRepository>();
builder.Services.AddScoped<PostInGenreRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<SoftwareRepository>();
builder.Services.AddScoped<PostInSoftwareRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<LikeRepository>();
builder.Services.AddScoped<CommentRepository>();
builder.Services.AddScoped<ImageInPostRepository>();
builder.Services.AddScoped<SubscriptionRepository>();
builder.Services.AddScoped<ChatMessageRepository>();
builder.Services.AddScoped<FeedTypeRepository>();

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

    var context = services.GetRequiredService<ArtbukContext>();
    var roleRepository = services.GetRequiredService<RoleRepository>();
    SampleData.Initialize(context, roleRepository);
    //try
    //{
    //    var context = services.GetRequiredService<ArtbukContext>();
    //    var roleRepository = services.GetRequiredService<RoleRepository>();
    //    SampleData.Initialize(context, roleRepository);
    //}
    //catch (Exception ex)
    //{
    //    var logger = services.GetRequiredService<ILogger<Program>>();
    //    logger.LogError(ex, "An error occurred seeding the DB.");
    //}
}

app.Run();
