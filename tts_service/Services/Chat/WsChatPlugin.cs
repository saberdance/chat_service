using Newtonsoft.Json;
using System.Buffers.Text;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Http.WebSockets;
using tsubasa;
using tts_service.Models.Chat;
using tts_service.Models.Protocol;
using tts_service.Models.Session;

namespace tts_service.Services.Chat
{
    public class WsChatPlugin : PluginBase, IWebSocketReceivedPlugin
    {
        private readonly ILog _logger;
        public WsChatPlugin(ILog logger)
        {
            _logger = logger;
        }
        public async Task OnWebSocketReceived(IWebSocket client, WSDataFrameEventArgs e)
        {
            switch (e.DataFrame.Opcode)
            {
                case WSDataType.Cont:
                    _logger.Info($"收到中间数据，长度为：{e.DataFrame.PayloadLength}");
                    return;

                case WSDataType.Text:
                    await HandleChatRequest(client, e);
                    return;

                case WSDataType.Binary:
                    LogBinaryInfo(e);
                    return;

                case WSDataType.Close:
                    CloseWebSocket(client);
                    return;

                case WSDataType.Ping:
                    //Do Nothing
                    break;

                case WSDataType.Pong:
                    //Do Nothing
                    break;

                default:
                    //Do Nothing
                    break;
            }

            await e.InvokeNext();
        }

        private async Task HandleChatRequest(IWebSocket client, WSDataFrameEventArgs e)
        {
            try
            {
                //如果格式不正确则进入catch逻辑，因此不在此判断text合法性
                ChatRequest request = JsonConvert.DeserializeObject<ChatRequest>(e.DataFrame.ToText());
                ChatContent userChatContent = CreateNewChatContent(request);
                LogChatRequest(request, userChatContent);
                ChatJob job = new ChatJob()
                {
                    Client = client,
                    UserId = request.user_id,
                    SessionId = request.session_id,
                    InnerContent = userChatContent,
                    OutputType = request.output_type
                };
                var jobRunner = new ChatJobRunner(job);
                await jobRunner.Run();
            }
            catch (Exception err)
            {
                _logger.Error("解析请求出错，JSON Parse Failed。原始文本：\n" + e.DataFrame.ToText());
                ChatResponse chatResponse = new ChatResponse()
                {
                    Code = -1,
                    Message = "JSON Parse Failed"
                };
                client.Send(JsonConvert.SerializeObject(chatResponse));
            }
            if (!client.Client.IsClient)
            {
                client.Send("我已收到");
            }
        }

        private void CloseWebSocket(IWebSocket client)
        {
            _logger.Info("远程请求断开");
            client.Close("断开");
        }

        private void LogBinaryInfo(WSDataFrameEventArgs e)
        {
            if (e.DataFrame.FIN)
            {
                _logger.Info($"收到二进制数据，长度为：{e.DataFrame.PayloadLength}");
            }
            else
            {
                _logger.Info($"收到未结束的二进制数据，长度为：{e.DataFrame.PayloadLength}");
            }
        }

        private void LogChatRequest(ChatRequest request, ChatContent content)
        {
            _logger.Info($"[Chat]--------新请求：用户:{request.user_id}|Session:{request.session_id}---------");
            _logger.Info($"[Chat]类型：{request.output_type}");
            _logger.Info($"[Chat]内容：{content.Content}");
            _logger.Info($"[Chat]---------------------------------------------------");
        }

        private ChatContent CreateNewChatContent(ChatRequest request)
        {
            return new ChatContent()
            {
                SessionId = request.session_id,
                Sender = SenderType.User,
                UserId = request.user_id,
                ContentType = ContentType.Text,
                DateTime = DateTime.Now,
                VoiceUrl = "",
                Content = Encoding.UTF8.GetString(Convert.FromBase64String(request.payload))
            }; 
        }
    }
}
