namespace tts_service.Models.Protocol
{
    public class CreateChatSessionRequest
    {
        public string? UserId { get; set; }
        public string? EngineName { get; set; }
        public string? VoiceName { get; set; }
        public string? Title { get; set; } = "";
    }
}
