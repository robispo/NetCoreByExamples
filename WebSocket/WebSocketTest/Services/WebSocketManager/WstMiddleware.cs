using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebSocketTest.Services.WebSocketManager
{
    public class WstMiddleware
    {
        private readonly RequestDelegate _next;
        private WstHandler _webSocketHandler { get; set; }

        public WstMiddleware(RequestDelegate next, WstHandler webSocketHandler)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var connection = await _webSocketHandler.OnConnected(context);
                if (connection != null)
                {
                    await _webSocketHandler.ListenConnection(connection);
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            }
        }
    }
}
