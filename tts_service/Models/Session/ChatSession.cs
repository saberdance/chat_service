using System.ComponentModel.DataAnnotations;

namespace tts_service.Models.Session
{
    public class ChatSession
    {
        [Key]
        public int Id { get; set; }
        public string? UserGuid { get; set; }
        public string? SessionId { get; set; } = Guid.NewGuid().ToString();
        public string? Title { get; set; } = "";
        public int ChatEngineId { get; set; }
        public int TtsEngineId { get; set; } 
        public int TtsVoiceId { get; set; }
        public int ContentCount { get; set; } = 0;
        public List<ChatContent> ChatContents { get; set; } = new List<ChatContent>();
        public int UserId { get; set; }
    }
}