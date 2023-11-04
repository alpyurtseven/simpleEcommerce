using EcommerceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddIdentity<Admin, AppRole>(options =>
options.SignIn.RequireConfirmedAccount = false)
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Admin/Login";
    options.AccessDeniedPath = "/Admin/AccessDenied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
    policy.RequireRole("Admin"));
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{

    var userManager = (UserManager<Admin>)scope.ServiceProvider
        .GetService(typeof(UserManager<Admin>));
    var roleManager = (RoleManager<AppRole>)scope.ServiceProvider
        .GetService(typeof(RoleManager<AppRole>));

    if (roleManager.FindByNameAsync("Admin").Result == null)
    {
        var adminRole = new AppRole
        {
            Name = "Admin"
        };
        roleManager.CreateAsync(adminRole).Wait();

    }

    if (userManager.FindByNameAsync("admin").Result == null)
    {
        Admin admin = new Admin
        {
            UserName = "admin",
            Email = "admin@admin.com",
        };

        IdentityResult result = userManager.CreateAsync(admin,
            "Admin123+").Result;

        if (result.Succeeded)
        {
            userManager.AddToRoleAsync(admin, "Admin").Wait();
        }
    }
}

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

