using Microsoft.EntityFrameworkCore;
using Entregable2_VilchezGuardia_JF.Data;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Sesiones (para Pregunta 4)
builder.Services.AddSession();

// Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration["Redis:ConnectionString"]);

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// =========================
// APLICAR MIGRACIONES AUTOMÁTICAS (SQLite)
// =========================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Crear rol Broker si no existe
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // Crear rol Broker
    if (!await roleManager.RoleExistsAsync("Broker"))
        await roleManager.CreateAsync(new IdentityRole("Broker"));

    // Opcional: asignar usuario admin al rol Broker
    var adminUser = await userManager.FindByEmailAsync("admin@broker.com");
    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Broker"))
        await userManager.AddToRoleAsync(adminUser, "Broker");
}

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession(); // activar sesiones

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

