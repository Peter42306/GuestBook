using GuestBook.Models;
using GuestBook.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args); // Создается объект builder, который используется для настройки и построения веб-приложения

// Все сессии работают поверх объекта IDistributedCache, и 
// ASP.NET Core предоставляет встроенную реализацию IDistributedCache

builder.Services.AddDistributedMemoryCache();// добавляем IDistributedMemoryCache
builder.Services.AddSession();  // Добавляем сервисы сессии

string? connection = builder.Configuration.GetConnectionString("DefaultConnection"); // Настройка подключения к базе данных
builder.Services.AddDbContext<GuestBookContext>(options => options.UseSqlServer(connection)); // Настраивается подключение к базе данных SQL Server с использованием строки подключения и добавляется контекст базы данных GuestBookContext

// Настройка аутентификации с использованием куков
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Указываем путь к странице входа
        options.AccessDeniedPath = "/Account/AccessDenied"; // Указываем путь к странице отказа в доступе
    });

builder.Services.AddControllersWithViews(); // Добавляются сервисы MVC, что позволяет работать с контроллерами и представлениями.

builder.Services.AddSaltGeneratorService(); // Сервис для генерирования соли
builder.Services.AddPasswordHasherService(); // Сервис для хеширования паролей
builder.Services.AddMyLoggerTxt();
builder.Services.AddMyLoggerXlsx();

var app = builder.Build(); // Построение и конфигурирование middleware. Создается объект app, представляющий построенное приложение.

app.UseSession();   // Middleware для работы с сессиями

app.UseStaticFiles(); // Middleware для работы со статическими файлами, обрабатывает запросы к файлам в папке wwwroot

app.UseRouting(); // Middleware для маршрутизации запросов

app.UseAuthentication(); // Middleware для аутентификации 

app.UseAuthorization(); // Middleware для авторизации

// Настройка маршрутов
// Конфигурируется стандартный маршрут, который указывает, что контроллер GuestBook и действие Index будут использоваться по умолчанию, если не указан другой маршрут
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=GuestBook}/{action=Index}/{id?}");

app.Run(); // Запуск приложения
