using System.ComponentModel.DataAnnotations;

namespace tts_service.Models.TtsData
{
    public class TtsVoice
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Engine { get; set; }
    }
}
