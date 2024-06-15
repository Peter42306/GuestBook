using GuestBook.Models;
using GuestBook.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args); // ��������� ������ builder, ������� ������������ ��� ��������� � ���������� ���-����������

// ��� ������ �������� ������ ������� IDistributedCache, � 
// ASP.NET Core ������������� ���������� ���������� IDistributedCache

builder.Services.AddDistributedMemoryCache();// ��������� IDistributedMemoryCache
builder.Services.AddSession();  // ��������� ������� ������

string? connection = builder.Configuration.GetConnectionString("DefaultConnection"); // ��������� ����������� � ���� ������
builder.Services.AddDbContext<GuestBookContext>(options => options.UseSqlServer(connection)); // ������������� ����������� � ���� ������ SQL Server � �������������� ������ ����������� � ����������� �������� ���� ������ GuestBookContext

// ��������� �������������� � �������������� �����
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // ��������� ���� � �������� �����
        options.AccessDeniedPath = "/Account/AccessDenied"; // ��������� ���� � �������� ������ � �������
    });

builder.Services.AddControllersWithViews(); // ����������� ������� MVC, ��� ��������� �������� � ������������� � ���������������.

builder.Services.AddSaltGeneratorService(); // ������ ��� ������������� ����
builder.Services.AddPasswordHasherService(); // ������ ��� ����������� �������
builder.Services.AddMyLoggerTxt();
builder.Services.AddMyLoggerXlsx();

var app = builder.Build(); // ���������� � ���������������� middleware. ��������� ������ app, �������������� ����������� ����������.

app.UseSession();   // Middleware ��� ������ � ��������

app.UseStaticFiles(); // Middleware ��� ������ �� ������������ �������, ������������ ������� � ������ � ����� wwwroot

app.UseRouting(); // Middleware ��� ������������� ��������

app.UseAuthentication(); // Middleware ��� �������������� 

app.UseAuthorization(); // Middleware ��� �����������

// ��������� ���������
// ��������������� ����������� �������, ������� ���������, ��� ���������� GuestBook � �������� Index ����� �������������� �� ���������, ���� �� ������ ������ �������
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=GuestBook}/{action=Index}/{id?}");

app.Run(); // ������ ����������
