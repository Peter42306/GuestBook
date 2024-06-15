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
    }
}
