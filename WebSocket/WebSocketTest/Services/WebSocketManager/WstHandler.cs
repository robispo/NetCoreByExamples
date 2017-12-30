﻿using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebSocketTest.Services.WebSocketManager
{
    public abstract class WstHandler
    {
        protected abstract int BufferSize { get; }

        private List<WstConnection> _connections = new List<WstConnection>();

        public List<WstConnection> Connections { get => _connections; }

        public async Task ListenConnection(WstConnection connection)
        {
            var buffer = new byte[BufferSize];

            while (connection.WebSocket.State == WebSocketState.Open)
            {
                var result = await connection.WebSocket.ReceiveAsync(
                    buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    await connection.ReceiveAsync(message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await OnDisconnected(connection);
                }
            }
        }

        public virtual async Task OnDisconnected(WstConnection connection)
        {
            if (connection != null)
            {
                _connections.Remove(connection);

                await connection.WebSocket.CloseAsync(
                    closeStatus: WebSocketCloseStatus.NormalClosure,
                    statusDescription: "Closed by the WebSocketHandler",
                    cancellationToken: CancellationToken.None);
            }
        }

        public abstract Task<WstConnection> OnConnected(HttpContext context);
    }
}
