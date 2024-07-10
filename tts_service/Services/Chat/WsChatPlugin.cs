using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Buffers.Text;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Http.WebSockets;
using tsubasa;
using tts_service.Db;
using tts_service.Models;
using tts_service.Models.Chat;
using tts_service.Models.Protocol;
using tts_service.Models.Session;

namespace tts_service.Services.Chat
{
    public class WsChatPlugin : PluginBase, IWebSocketReceivedPlugin
    {
        private readonly ILog _logger;
        private readonly ChatContext _context;
        private readonly string _db_connect_string = "Data Source=saberdance.cc;Initial Catalog=tts;Persist Security Info=True;User ID=sa;Password=198731Shiki;Encrypt=False;Trust Server Certificate=True";
        public WsChatPlugin(ILog logger)
        {
            _logger = logger;
            var contextOptions = new DbContextOptionsBuilder<ChatContext>()
                .UseSqlServer(_db_connect_string).Options;
            _context = new ChatContext(contextOptions);
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
                _logger.Info("收到文本数据:"+e.DataFrame.ToText());
                //如果格式不正确则进入catch逻辑，因此不在此判断text合法性
                ChatRequest request = JsonConvert.DeserializeObject<ChatRequest>(e.DataFrame.ToText());
                
                //var user = await _context.Users.Where(o=>o.Guid == request.user_id)(request.user_id);
                //if (user == default)
                //{
                //    client.Send(JsonConvert.SerializeObject(new BaseResponse<string>() { Code = ProtocolErrorCode.UserNotFound,Message = "User not found"}));
                //}

                //！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
                //以下代码为逻辑接口测试用，须尽快删除
                DoLogicTest(client);
                return;
                //！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
                //ChatContent userChatContent = CreateNewChatContent(request,user);
                //LogChatRequest(request, userChatContent);
                //ChatJob job = new ChatJob()
                //{
                //    Client = client,
                //    UserId = user!.Id,
                //    UserGuid = user!.Guid,
                //    SessionId = request.session_id,
                //    InnerContent = userChatContent,
                //    OutputType = request.output_type
                //};
                //var jobRunner = new ChatJobRunner(job, _context);
                //await jobRunner.Run();
            }
            catch (Exception err)
            {
                _logger.Error("解析请求出错，JSON Parse Failed。原始文本：\n" + e.DataFrame.ToText());
                BaseResponse<string> chatResponse = new BaseResponse<string>()
                {
                    Code = ProtocolErrorCode.InvalidParameter,
                    Message = "Request JSON Parse Error"
                };
                client.Send(JsonConvert.SerializeObject(chatResponse));
            }
            //if (!client.Client.IsClient)
            //{
            //    client.Send("Recived");
            //}
        }

        private void DoLogicTest(IWebSocket client)
        {
            ChatResponse content = new ChatResponse()
            {
                user_id = "123",
                session_id = 1,
                content_id = 2,
                output_type = "voice",
                is_end = true,
                payload = "这是一句测试回复测试测试回复;嗷嗷嗷嗷哦嗷嗷嗷嗷嗷嗷嗷嗷嗷嗷哦嗷嗷嗷嗷嗷哦嗷嗷嗷嗷嗷嗷嗷嗷。",
                voice_url = "/StaticFiles/Videos/3_1720429115.mp3",

            };
            client.Send(JsonConvert.SerializeObject(new SuccessResponse<ChatResponse>(content)));
        }

        private void CloseWebSocket(IWebSocket client)
        {
            _logger.Info("远程请求断开");
            client.Close("Disconnect");
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

        private ChatContent CreateNewChatContent(ChatRequest request,User user)
        {
            return new ChatContent()
            {
                SessionId = request.session_id,
                Sender = SenderType.User,
                UserId = user.Id,
                UserGuid = user.Guid,
                ContentType = ContentType.Text,
                DateTime = DateTime.Now,
                VoiceUrl = "",
                Content = Encoding.UTF8.GetString(Convert.FromBase64String(request.payload))
            }; 
        }
    }
}
