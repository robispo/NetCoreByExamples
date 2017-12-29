using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace UserWebApi.Services
{
    public class JwtMiddleware
    {
        readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context, IJwtService jwtService)
        {
            jwtService.ValidateAndRenewToken(context);
            
            return 
                this._next(context);
        }
    }

    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseValidateAndRenewToken(this IApplicationBuilder builder)
        {
            return
                builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
