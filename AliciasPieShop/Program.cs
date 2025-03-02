using AliciasPieShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AliciasPieShopDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AliciasPieShopDbContextConnection' not found.");

// REGISTER SERVICES WITH DI CONTAINER

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// add shopping cart service. we invoke get cart every time we use the services, passing the service provider as a parameter
builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp));
// add sessions service as it is used in our shopping cart implementation
builder.Services.AddSession();
// add HttpContextAcessor service as we use its interface in the GetCart method.
builder.Services.AddHttpContextAccessor();


// add framework services for .NET Core MVC, do not return responses containing endless reference cycles between tightly-coupled objects
builder.Services.AddControllersWithViews().AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; }); 


builder.Services.AddRazorPages();             // add framwork servcies for Razor Pages
builder.Services.AddDbContext<AliciasPieShopDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:AliciasPieShopDbContextConnection"]);
});

builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<AliciasPieShopDbContext>();


var app = builder.Build();                    // build application instance using webappbuilder inst

// CONSTRUCT MIDDLEWARE PIPELINE ****IN THE DESIRED ORDER****

app.UseStaticFiles();           // pipeline component 1: look in wwwroot folder for static files
app.UseSession();               // pipeline component 2: we need to slap this in here because we use sessions, which use cookies or something
app.UseAuthentication();        // add middleware for authentication (logging in/out etc)
app.UseAuthorization();         // add middleware for authorization (RBAC for resources/endpoints)

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    // pipeline component 5: show errors page if we are in development mode
}

app.MapDefaultControllerRoute();        // endpoint middleware (component 6) - let MVC controllers handle requests
                                        // {controller=Home}/{action=Index}/{id?}

app.MapRazorPages();                    // add routing for razor pages, with Pages folder as the root endpoint

DbInitializer.Seed(app);                // fill our database with seed data if it is otherwise empty! (DbInitializer is a static class)

app.Run();          // all sorted, let's run this thing baby
