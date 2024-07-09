using System.Runtime.CompilerServices;
using tts_service.Models.Chat;
using tts_service.Models.Session;

namespace tts_service.Services.Chat.AIChater
{
    public interface IChater
    {
        public Task<ChatContent> Chat(ChatJob job,ChatContent content,string inputs);
    }
}
