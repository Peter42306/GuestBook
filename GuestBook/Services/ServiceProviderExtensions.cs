using System.Runtime.CompilerServices;

namespace GuestBook.Services
{
    public static class ServiceProviderExtensions
    {
        public static void AddSaltGeneratorService(this IServiceCollection services)
        {
            services.AddTransient<ISaltGenerator, SaltGenerator>();
        }

        public static void AddPasswordHasherService(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();
        }

        public static void AddMyLoggerTxt(this IServiceCollection services)
        {            
            services.AddSingleton<MyLoggerTXT>(provider=>new MyLoggerTXT("myLogger.txt"));
        }

        public static void AddMyLoggerXlsx(this IServiceCollection services)
        {            
            services.AddSingleton<MyLoggerXlsx>(provider => new MyLoggerXlsx("myLogger.xlsx"));
        }
    }
}
