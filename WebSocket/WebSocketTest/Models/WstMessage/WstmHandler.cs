using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebSocketTest.Services.WebSocketManager;

namespace WebSocketTest.Models.WstMessage
{
    public class WstmHandler : WstHandler
    {
        protected override int BufferSize { get => 1024 * 4; }

        public override async Task<WstConnection> OnConnected(HttpContext context)
        {
            string name = context.Request.Query["Name"];

            if (!string.IsNullOrWhiteSpace(name))
            {
                var connection = Connections.FirstOrDefault(m => ((WstmConnection)m).NickName == name);

                if (connection == null)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    connection = new WstmConnection(this)
                    {
                        NickName = name,
                        WebSocket = webSocket
                    };

                    Connections.Add(connection);
                }

                return 
                    connection;
            }

            return null;
        }
    }
}
