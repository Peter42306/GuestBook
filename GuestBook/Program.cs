using GuestBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// ��� ������ �������� ������ ������� IDistributedCache, � 
// ASP.NET Core ������������� ���������� ���������� IDistributedCache
builder.Services.AddDistributedMemoryCache();// ��������� IDistributedMemoryCache
builder.Services.AddSession();  // ��������� ������� ������

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<GuestBookContext>(options => options.UseSqlServer(connection));

// ��������� �������������� � �������������� �����
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // ��������� ���� � �������� �����
        options.AccessDeniedPath = "/Account/AccessDenied"; // ��������� ���� � �������� ������ � �������
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();   // ��������� middleware-��������� ��� ������ � ��������

app.UseStaticFiles(); // ������������ ������� � ������ � ����� wwwroot

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=GuestBook}/{action=Index}/{id?}");

app.Run();
