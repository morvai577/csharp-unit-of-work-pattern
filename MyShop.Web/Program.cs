using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyShop.Domain.Models;
using MyShop.Infrastructure;
using MyShop.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddMvc();
builder.Services.AddControllers(options => options.EnableEndpointRouting = false);

CreateInitialDatabase();

builder.Services.AddTransient<ShoppingContext>();
builder.Services.AddTransient<IRepository<Order>, OrderRepository>();
builder.Services.AddTransient<IRepository<Product>, ProductRepository>();
builder.Services.AddTransient<IRepository<Customer>, CustomerRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Order/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Order}/{action=Index}/{id?}");
});

app.Run();

void CreateInitialDatabase()
{
    using var context = new ShoppingContext();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    var camera = new Product { Name = "Canon EOS 70D", Price = 599m };
    var microphone = new Product { Name = "Shure SM7B", Price = 245m };
    var light = new Product { Name = "Key Light", Price = 59.99m };
    var phone = new Product { Name = "Android Phone", Price = 259.59m };
    var speakers = new Product { Name = "5.1 Speaker System", Price = 799.99m };

    context.Products.Add(camera);
    context.Products.Add(microphone);
    context.Products.Add(light);
    context.Products.Add(phone);
    context.Products.Add(speakers);

    context.SaveChanges();
}