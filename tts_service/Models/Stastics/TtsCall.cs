using System.ComponentModel.DataAnnotations;

namespace tts_service.Models.Stastics
{
    public class TtsCall
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int EngineId { get; set; }
        public string? EngineName { get; set; }
        public int VoiceId { get; set; }
        public string? VoiceName { get; set; }
        public int CallCount { get; set; } = 0;
    }
}
