using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mb_back.Controllers
{
    [Route("ws/gg")]
    public class SocketController : Controller
    {
        //private readonly ApplicationDbContext _context;

        //public SocketController(ApplicationDbContext context)
        //{
        //    _context = context;

        //}

        /// <summary>
        /// Нужно для коннекта клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                
                var buffer = new byte[1024 * 4];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var str = System.Text.Encoding.Default.GetString(buffer);
                Console.WriteLine(str);
                await Echo(HttpContext, webSocket);
            }

            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        // Тест возврата полученных сообщений
        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),
                CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count),
                    result.MessageType,
                    result.EndOfMessage,
                    CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),
                    CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value,
                result.CloseStatusDescription,
                CancellationToken.None);
        }
    }
}
