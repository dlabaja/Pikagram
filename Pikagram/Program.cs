using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pikagram;
using Pikagram.Models;
using Pikagram.Services;

var builder = WebApplication.CreateBuilder(args);

// nejsem si jistej jestli to něco změnilo, pořád musím refreshovat stránku
// ale už to nechce tak často restart, tak asi jo
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// init DbContextu
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // Connection String mám v appsettings.json
    options.UseLazyLoadingProxies().UseSqlite(builder.Configuration.GetConnectionString("MainDb"));
});

builder.Services.AddRouting(options =>
{
    // url s uppercasem je odporný
    options.LowercaseUrls = true;
});

// konfigurace identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789-._";
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/";
    options.AccessDeniedPath = "/";
});

// moje vlastní service na správu médií
builder.Services.AddScoped<MediaService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

// error handling stránka
app.UseStatusCodePagesWithReExecute("/error/{0}");

// posílá média přidaný za běhu
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// výchozí cesta
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();