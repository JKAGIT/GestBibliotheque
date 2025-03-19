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

//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));




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

//app.UseAuthorization();
//app.UseAuthentication();

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
