using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Curso.Servicios;
using Curso.Data;
using Curso.Servicios.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection; // 👈 Asegúrate de incluir esto

var builder = WebApplication.CreateBuilder(args);

// Base de datos
builder.Services.AddDbContext<Curso.Data.Dbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Dbcontext")
        ?? throw new InvalidOperationException("Connection string 'DbContext' not found.")));

// Autenticación por cookies 👇
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Login";
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Shared/Error";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });


// Autorización (si usas [Authorize])
builder.Services.AddAuthorization();

// Otros servicios
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHostedService<NotificationService>();
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseNotyf();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 👇 Muy importante: este orden
app.UseAuthentication(); // Primero autenticación
app.UseAuthorization();  // Luego autorización

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
