using DoubaoTtsSdk;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TouchSocket.Core;
using tts_service.Db;
using tts_service.Models.Chat;
using tts_service.Models.Protocol;
using tts_service.Models.Session;
using tts_service.Services.Chat.AIChater;

namespace tts_service.Services.Chat
{
    public class ChatJobRunner
    {
        private ChatJob _job;
        private ILog _logger;
        
        private ChatContext _context;

        public ChatJobRunner(ChatJob job,ChatContext context)
        {
            _context = context;
            _job = job;
            
        }

        public async Task<bool> Run()
        {
            _logger.Info($"Start Chat Job:{ _job.UserId}|{_job.SessionId}");
            ChatSession session = await _context.ChatSessions.FindAsync(_job.SessionId);
            if (session == default)
            {
                SendErrorResponse("Error Session Id:"+_job.SessionId);
                return false;
            }
            ChatEngine engine = await _context.ChatEngines.FindAsync(session.ChatEngineId);
            if (engine == default)
            {
                SendErrorResponse("Error Engine Id:" + session.ChatEngineId);
                return false;
            }
            ChatContent outerContent = new ChatContent()
            {
                SessionId = session.Id,
                UserId = _job.UserId,
                Sender = SenderType.Bot,
                DateTime = DateTime.Now,
                ContentType = _job.OutputType == "voice"? ContentType.Voice : ContentType.Text,
            };
            string innerInputs = GenerateInnterInput(session);
            outerContent = await ChaterSelector.GetChater(engine.Name!).Chat(_job, outerContent,innerInputs);
            session.ContentCount++;
            _context.ChatContents.Add(outerContent);
            await _context.SaveChangesAsync();
            _logger.Info($"End Chat Job:{ _job.UserId}|{_job.SessionId}");
            return true;
        }

        private string GenerateInnterInput(ChatSession session)
        {
            //TODO：拼合历史记录和当前输入为一个Input
            throw new NotImplementedException();
        }

        private void SendErrorResponse(string? msg = null)
        {
            BaseResponse<string> response = new BaseResponse<string>() 
            {
                Code = ProtocolErrorCode.WebsocketError,
                Message = msg ?? "Websocket Error"
            };
            _job.Client.Send(JsonConvert.SerializeObject(msg));
        }
    }
}
