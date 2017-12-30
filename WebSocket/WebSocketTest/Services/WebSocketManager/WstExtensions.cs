using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebSocketTest.Services.WebSocketManager
{
    public static class WstExtensions
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app, PathString path, WstHandler handler)
        {
            return 
                app.Map(path, (_app) => _app.UseMiddleware<WstMiddleware>(handler));
        }

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            var handlerBaseType = typeof(WstHandler);

            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == handlerBaseType)
                {
                    services.AddSingleton(type);
                }
            }

            return 
                services;
        }
    }
}
