using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RolesWithoutIdentity.Data;
using RolesWithoutIdentity.Helpers;
using RolesWithoutIdentity.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default") ?? throw new InvalidOperationException("Connection string 'DefaultContext' not found.")));

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("Admin", policy => policy.Requirements.Add(new RoleRequirement(new string[] { "ADMIN" })));
    opt.AddPolicy("Poweruser", policy => policy.Requirements.Add(new RoleRequirement(new string[] { "ADMIN", "POWERUSER" })));
    opt.AddPolicy("User", policy => policy.Requirements.Add(new RoleRequirement(new string[] { "ADMIN", "POWERUSER", "USER" })));
});

//needs to be scoped, otherwise DI of DbContext doesn't work in customauthorization
builder.Services.AddScoped<IAuthorizationHandler, CustomAuthorization>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
