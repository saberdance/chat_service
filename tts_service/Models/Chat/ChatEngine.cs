using System.ComponentModel.DataAnnotations;
using tts_service.Models.TtsData;

namespace tts_service.Models.Chat
{
    public class ChatEngine
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int TokenRemain { get; set; }
        public int CallCount { get; set; }
        public bool Avaliable { get; set; }
    }
}
