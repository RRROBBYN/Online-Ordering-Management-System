using _4IdiotsInc.Components;
using _4IdiotsInc.Components.Service;
using _4IdiotsInc.Data;
using _4IdiotsInc.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ? Use DbContextFactory so each repository call gets a fresh DbContext instance
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Optional: Keep this if other services (not factories) need AppDbContext
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repository
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ProductsRepository>();
builder.Services.AddScoped<ProductCategoryRepository>();
builder.Services.AddScoped<SearchService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<CheckoutService>();
builder.Services.AddScoped<OrdersRepository>();
builder.Services.AddScoped<OrderStatusRepository>();
builder.Services.AddScoped<ShippingAddressRepository>();
builder.Services.AddScoped<CheckoutService>();
builder.Services.AddScoped<UserSessionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
