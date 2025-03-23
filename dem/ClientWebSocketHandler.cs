using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dem
{
    public class ClientWebSocketHandler : IDisposable
    {
        public delegate void ClientWebSocketReceiveMessages(string message);
        public event ClientWebSocketReceiveMessages OnReceiveMessage;

        private readonly Uri _uri = new Uri("ws://217.114.2.102:8088/reviews");
        private ClientWebSocket _webSocket = new();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ClientWebSocketHandler()
        {
            _ = InitWebSocket();
        }

        public async Task SendMessage(string message)
        {
            await _webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task InitWebSocket()
        {
            _webSocket = new ClientWebSocket();
            await _webSocket.ConnectAsync(_uri, CancellationToken.None);
            while (_webSocket.State == WebSocketState.Open)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024 * 4]);
                var result = await _webSocket.ReceiveAsync(buffer, CancellationToken.None);
                OnReceiveMessage?.Invoke(Encoding.UTF8.GetString(buffer.Array, 0, result.Count));
            }
        }
    }
}
