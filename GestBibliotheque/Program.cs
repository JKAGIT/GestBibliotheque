using GestBibliotheque.Donnee;
using GestBibliotheque.Repositories;
using GestBibliotheque.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy = options.DefaultPolicy;
//});


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


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Categories}/{action=Index}/{id?}")
//    .WithStaticAssets();




app.Run();
