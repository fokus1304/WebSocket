using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace WebSocketsChatApi.Controllers;
[ApiController]
public class WebSocketChatController : ControllerBase
{
    private static List<WebSocket> _webSockets = new();
    [Route("/chat")]
    public async Task Chat()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            _webSockets.Add(webSocket);

            await ResendMessages(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    public async Task ResendMessages(WebSocket senderWebSocket)
    {
        while (senderWebSocket.State == WebSocketState.Open)
        {
            ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[1024 * 4]);
            var result = await senderWebSocket.ReceiveAsync(buffer, CancellationToken.None);
            foreach (var websocket in _webSockets)
            {
                if (senderWebSocket == websocket) continue;
                await websocket.SendAsync(buffer.Array, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}