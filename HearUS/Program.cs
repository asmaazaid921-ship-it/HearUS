using System.Globalization;
using HearUS.Application;
using HearUS.Application.Common.Interfaces;
using HearUS.Domain.Constants;
using HearUS.Domain.Entities;
using HearUS.Hubs;
using HearUS.Infrastructure;
using HearUS.Infrastructure.Persistence;
using HearUS.Infrastructure.Persistence.Seed;
using HearUS.Web.Filters;
using HearUS.Web.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("لم يتم العثور على سلسلة الاتصال DefaultConnection.");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "HearUS.Auth";
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppPolicies.ExpertsOnly,
        policy => policy.RequireRole(AppRoles.Expert, AppRoles.Admin));
});

builder.Services.AddScoped<IChatNotifier, SignalRChatNotifier>();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews(options => options.Filters.Add<ExceptionHandlingFilter>());

var arabicCulture = new CultureInfo("ar-EG");
CultureInfo.DefaultThreadCurrentCulture = arabicCulture;
CultureInfo.DefaultThreadCurrentUICulture = arabicCulture;

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStatusCodePagesWithReExecute("/Home/HttpStatus", "?code={0}");

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapHub<ChatHub>("/chatHub");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await DbSeeder.SeedAsync(app.Services);

app.Run();
