using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pikagram;
using Pikagram.Models;
using Pikagram.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlite(builder.Configuration.GetConnectionString("MainDb"));
});

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true; // url s uppercasem je odporný
});

builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => { options.Password.RequiredLength = 8; })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/";
    //options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddScoped<MediaService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();