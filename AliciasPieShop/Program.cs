using AliciasPieShop.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// REGISTER SERVICES WITH DI CONTAINER

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();

builder.Services.AddControllersWithViews();   // add framework services for .NET Core MVC
builder.Services.AddDbContext<AliciasPieShopDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:AliciasPieShopDbContextConnection"]);
});


var app = builder.Build();                    // build application instance using webappbuilder inst

// CONSTRUCT MIDDLEWARE PIPELINE ****IN THE DESIRED ORDER****

app.UseStaticFiles();               // pipeline component 1: look in wwwroot folder for static files

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    // pipeline component 2: show errors page if we are in development mode
}

app.MapDefaultControllerRoute();        // endpoint middleware (component 3) - let MVC controllers handle requests

DbInitializer.Seed(app);                // fill our database with seed data if it is otherwise empty! (DbInitializer is a static class)

app.Run();          // all sorted, let's run this thing baby
