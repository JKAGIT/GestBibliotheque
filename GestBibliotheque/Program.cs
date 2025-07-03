using GestBibliotheque.Donnee;
using GestBibliotheque.Repositories;
using GestBibliotheque.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Pour les plateformes comme Render et Railway (port dynamique)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// Serilog pour la journalisation
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

// Base de données
builder.Services.AddDbContext<GestBibliothequeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GestBibliothequeDbConnect")));

// Injections de dépendances
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IEntityValidationService<>), typeof(EntityValidationService<>));
builder.Services.AddScoped(typeof(IRecherche<>), typeof(Recherche<>));
builder.Services.AddScoped<ICategories, CategoriesService>();
builder.Services.AddScoped<IEmprunts, EmpruntsService>();
builder.Services.AddScoped<ILivres, LivresService>();
builder.Services.AddScoped<IReservations, ReservationsService>();
builder.Services.AddScoped<IUsagers, UsagersService>();
builder.Services.AddScoped<IUtilisateurs, UtilisateursService>();
builder.Services.AddScoped<IRetours, RetoursService>();
builder.Services.AddScoped<GenerateurMatriculeUnique>();

// Authentification Azure
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuration de l'app
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // Ne pas rediriger vers HTTPS en Docker (Render/Railway)
    // app.UseHttpsRedirection();
    // app.UseHsts();
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




//Version avant render et railway
//using GestBibliotheque.Donnee;
//using GestBibliotheque.Repositories;
//using GestBibliotheque.Services;
//using Microsoft.AspNetCore.Authentication.OpenIdConnect;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Identity.Web;
//using Serilog;


//var builder = WebApplication.CreateBuilder(args);
//// Configure Serilog
//Log.Logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .CreateLogger();
//builder.Host.UseSerilog();


//builder.Services.AddDbContext<GestBibliothequeDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("GestBibliothequeDbConnect")));

//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped(typeof(IEntityValidationService<>), typeof(EntityValidationService<>));
//builder.Services.AddScoped(typeof(IRecherche<>), typeof(Recherche<>));
//builder.Services.AddScoped<ICategories, CategoriesService>();
//builder.Services.AddScoped<IEmprunts, EmpruntsService>();
//builder.Services.AddScoped<ILivres, LivresService>();
//builder.Services.AddScoped<IReservations, ReservationsService>();
//builder.Services.AddScoped<IUsagers, UsagersService>();
//builder.Services.AddScoped<IUtilisateurs, UtilisateursService>();
//builder.Services.AddScoped<IRetours, RetoursService>();


//builder.Services.AddScoped<GenerateurMatriculeUnique>();

//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));


//// Add services to the container.
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}



//app.UseHttpsRedirection();
//app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapStaticAssets();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}")
//    .WithStaticAssets();

//app.Run();
