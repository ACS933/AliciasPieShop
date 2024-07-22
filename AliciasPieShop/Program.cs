using AliciasPieShop.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// REGISTER SERVICES WITH DI CONTAINER

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();

// add shopping cart service. we invoke get cart every time we use the services, passing the service provider as a parameter
builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp));
// add sessions service as it is used in our shopping cart implementation
builder.Services.AddSession();
// add HttpContextAcessor service as we use its interface in the GetCart method.
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();   // add framework services for .NET Core MVC
builder.Services.AddDbContext<AliciasPieShopDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:AliciasPieShopDbContextConnection"]);
});


var app = builder.Build();                    // build application instance using webappbuilder inst

// CONSTRUCT MIDDLEWARE PIPELINE ****IN THE DESIRED ORDER****

app.UseStaticFiles();           // pipeline component 1: look in wwwroot folder for static files
app.UseSession();               // pipeline component 2: we need to slap this in here because we use sessions, which use cookies or something

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    // pipeline component 3: show errors page if we are in development mode
}

app.MapDefaultControllerRoute();        // endpoint middleware (component 4) - let MVC controllers handle requests
                                        // {controller=Home}/{action=Index}/{id?}

DbInitializer.Seed(app);                // fill our database with seed data if it is otherwise empty! (DbInitializer is a static class)

app.Run();          // all sorted, let's run this thing baby
