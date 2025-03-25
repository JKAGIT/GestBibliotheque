using GestBibliotheque.Donnee;
using GestBibliotheque.Repositories;
using GestBibliotheque.Services;
using GestBibliotheque.Utilitaires;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) 
    .CreateLogger();
builder.Host.UseSerilog(); 


builder.Services.AddDbContext<GestBibliothequeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GestBibliothequeDbConnect")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<CategoriesService>();
builder.Services.AddScoped<LivresService>();
builder.Services.AddScoped<UtilisateursService>();
builder.Services.AddScoped<GenerateurMatriculeUnique>();
builder.Services.AddScoped<UsagersService>();
builder.Services.AddScoped<EmpruntsService>();
builder.Services.AddScoped<RetoursService>();
builder.Services.AddScoped<ReservationsService>();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));


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
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
