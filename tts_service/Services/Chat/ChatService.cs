﻿using TouchSocket;
using TouchSocket.Core;
using TouchSocket.Http;
using TouchSocket.Sockets;
using tsubasa;

namespace tts_service.Services.Chat
{
    public class ChatService
    {
        private HttpService? _service;
        private readonly string? _testWsToken = "Cotnetwork_Coooooool_Wocao";
        public void Init(int port = 5155, string wsUrl = "/ws")
        {
            try
            {
                _service = new HttpService();
                _service.Setup(new TouchSocketConfig()
                    .SetListenIPHosts(port)
                    .ConfigureContainer((container) =>
                    {
                        container.AddConsoleLogger();
                    })
                    .ConfigurePlugins((plugins) =>
                    {
                        plugins.UseWebSocket()
                        .SetVerifyConnection(VerifyConnection)
                        .UseAutoPong();
                        //这里注入了接豆包的Plugin服务
                        plugins.Add<WsChatPlugin>();
                    })
                    );
                _service.Start();
                _service.Logger.Info("Chat WebSocket服务启动");
            }
            catch (Exception e)
            {
                Logger.ConsoleLog("启动Chat WebSocket失败:" + e.Message);
                throw;
            }

        }

        private bool VerifyConnection(IHttpSocketClient client, TouchSocket.Http.HttpContext context)
        {
            if (!context.Request.IsUpgrade())//如果不包含升级协议的header，就直接返回false。
            {
                return false;
            }
            if (context.Request.UrlEquals("/ws"))//以此连接，则需要从header传入token才可以连接
            {
                if (context.Request.Headers.Get("Authorization") == _testWsToken)
                {
                    return true;
                }
                else
                {
                    context.Response
                        .SetStatus(403, "token不正确")
                        .Answer();
                }
            }
            return false;
        }
    }
}
