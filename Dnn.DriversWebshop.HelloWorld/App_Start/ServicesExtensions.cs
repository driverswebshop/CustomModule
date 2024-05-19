using Microsoft.Extensions.DependencyInjection;
using DriversWebshop.Dnn.Dnn.DriversWebshop.HelloWorld.Data;

namespace Dnn.DriversWebshop.HelloWorld
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddCustomModuleServices(this IServiceCollection services)
        {
            services.AddScoped<CustomModuleRepository>();
            return services;
        }
    }
}