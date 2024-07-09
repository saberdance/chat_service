using TouchSocket.Http.WebSockets;
using tts_service.Models.Session;

namespace tts_service.Models.Chat
{
    public class ChatJob
    {
        public int Id { get; set; }
        public IWebSocket Client { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public string? OutputType { get; set; }
        public ChatContent? InnerContent { get; set; }
    }
}
