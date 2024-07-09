using System.ComponentModel.DataAnnotations;

namespace tts_service.Models.TtsService
{
    public class DoubaoTtsAccount
    {
        [Key]
        public int Id { get; set; }
        public string? Appid { get; set; }
        public string? Token { get; set; }
        public string? Cluster { get; set; }
        public string? Host { get; set; }
    }
}
