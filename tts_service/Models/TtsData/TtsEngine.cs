using System.ComponentModel.DataAnnotations;

namespace tts_service.Models.TtsData
{
    public class TtsEngine
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int TokenRemain { get; set; }
        public int CallCount { get; set; }
        public bool Avaliable { get; set; }
        public List<TtsVoice> Vocies { get; set; } = new List<TtsVoice>();
    }
}
