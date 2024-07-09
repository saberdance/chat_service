using System.ComponentModel.DataAnnotations;

namespace tts_service.Models.AiService
{
    public class DoubaoChatAccount
    {
        [Key]
        public int Id { get; set; }
        //AK
        public string? AppKey { get; set; }
        //SK
        public string? SecretKey { get; set; }
    }
}
