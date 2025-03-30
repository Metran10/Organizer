using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Organizer.Data;


var builder = WebApplication.CreateBuilder(args);


// DB Connection Configuration
var connnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connnectionString));

//Identity Configuration
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); 

var app = builder.Build();


// Add Roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    if(!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    if(!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }

    var defaultAdmin = await userManager.FindByEmailAsync("admin@admin.com");
    if (defaultAdmin != null && !(await userManager.IsInRoleAsync(defaultAdmin, "Admin")))
    {
        await userManager.AddToRoleAsync(defaultAdmin, "Admin");
    }

}


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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
