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
        private readonly string _db_connect_string = "Data Source=saberdance.cc;Initial Catalog=tts;Persist Security Info=True;User ID=sa;Password=198731Shiki;Encrypt=False;Trust Server Certificate=True";
        private ChatContext _context;

        public ChatJobRunner(ChatJob job)
        {
            _job = job;
            var contextOptions = new DbContextOptionsBuilder<ChatContext>()
                .UseSqlServer(_db_connect_string).Options;
            _context = new ChatContext(contextOptions);
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
            outerContent = await ChaterSelector.GetChater(engine.Name!).Chat(_job, outerContent);
            session.ContentCount++;
            _context.ChatContents.Add(outerContent);
            await _context.SaveChangesAsync();
            _logger.Info($"End Chat Job:{ _job.UserId}|{_job.SessionId}");
            //TODO:继续写ChatJob
            return true;
        }

        private void SendErrorResponse(string msg)
        {
            ChatResponse chatResponse = new ChatResponse()
            {
                Code = -1,
                Message = msg
            };
            _job.Client.Send(JsonConvert.SerializeObject(chatResponse));
        }
    }
}
