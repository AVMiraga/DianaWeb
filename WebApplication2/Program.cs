using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

var app = builder.Build();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Dash}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
