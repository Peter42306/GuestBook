using GuestBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Все сессии работают поверх объекта IDistributedCache, и 
// ASP.NET Core предоставляет встроенную реализацию IDistributedCache
builder.Services.AddDistributedMemoryCache();// добавляем IDistributedMemoryCache
builder.Services.AddSession();  // Добавляем сервисы сессии

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<GuestBookContext>(options => options.UseSqlServer(connection));

// Настройка аутентификации с использованием куков
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Указываем путь к странице входа
        options.AccessDeniedPath = "/Account/AccessDenied"; // Указываем путь к странице отказа в доступе
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();   // Добавляем middleware-компонент для работы с сессиями

app.UseStaticFiles(); // обрабатывает запросы к файлам в папке wwwroot

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=GuestBook}/{action=Index}/{id?}");

app.Run();
