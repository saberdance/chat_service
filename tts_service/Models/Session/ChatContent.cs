using System.ComponentModel.DataAnnotations;

namespace tts_service.Models.Session
{
    public enum ContentType
    {
        Text,
        Voice
    }
    public enum SenderType
    {
        User,
        Bot,
        System
    }
    public class ChatContent
    {
        [Key]
        public int Id { get; set; }
        public string? UserGuid { get; set; }
        public string? ContentId { get; set; } = Guid.NewGuid().ToString();
        public SenderType Sender { get; set; } = SenderType.User;
        public ContentType ContentType { get; set; } = ContentType.Text;
        public string? Content { get; set; }
        public string? VoiceUrl { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public int SessionId { get; set; }

        public string SenderName() => Sender switch
        {
            SenderType.User => "user",
            SenderType.Bot => "assistant",
            SenderType.System => "system",
            _ => "Unknown"
        };
    }
}
