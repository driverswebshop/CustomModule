using Microsoft.Extensions.DependencyInjection;
using DriversWebshop.Dnn.Dnn.DriversWebshop.HelloWorld.Data;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;
using System;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Dnn.DriversWebshop.HelloWorld.App_Start.Startup), "PreStart")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Dnn.DriversWebshop.HelloWorld.App_Start.Startup), "PostStart")]

namespace Dnn.DriversWebshop.HelloWorld.App_Start
{
    public static class Startup
    {
        public static void PreStart()
        {
            // Pre-start initialization code if needed
        }

        public static void PostStart()
        {
            // Post-start initialization code if needed
            RegisterServices();
            RegisterRoutes(RouteTable.Routes);
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddCustomModuleServices();

            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();

            // Set the dependency resolver
            DependencyResolver.SetResolver(new ServiceProviderDependencyResolver(serviceProvider));
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Item", action = "Index", id = UrlParameter.Optional }
            );
        }
    }

    public class ServiceProviderDependencyResolver : IDependencyResolver
    {
        private readonly ServiceProvider _serviceProvider;

        public ServiceProviderDependencyResolver(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _serviceProvider.GetServices(serviceType);
        }
    }
}
