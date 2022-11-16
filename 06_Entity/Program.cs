using _06_Entity.DAO;
using _06_Entity.Models;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// -- NLog
// Installer NLog.Web.AspNetCore
// Cr�er nlog.config
// dans les propri�t�s de nlog.config choisir "copier dans le r�pertoire de sortie" : "copier si plus r�cent"
// Ajoute Nlog � liste des "Logging Provider"

builder.Logging.AddNLog();
// Add services to the container.
builder.Services.AddControllersWithViews();

// -------------------------------------------
// ON S'ABONNE AU SERVICE AddDbContext
// -------------------------------------------

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ----------------- Pour acc�der � la couche d'acc�s aux donn�es des produits
builder.Services.AddScoped<IProductDAO, ProductDAO>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseStatusCodePagesWithReExecute("/Error/{0}"); // redirige vers l'url /Error/{StatusCode}
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
