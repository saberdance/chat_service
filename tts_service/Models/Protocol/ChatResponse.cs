using Microsoft.AspNetCore.Components.Web;

namespace tts_service.Models.Protocol
{
    public class ChatResponse:BaseResponse
    {
        public int user_id { get; set; }
        public int session_id { get; set; }
        public int content_id { get;set; }
        public string? output_type { get; set; }
        public bool is_end { get; set; } = false;
        public string? payload { get; set; }
        public string? voice_url { get; set; }
    }
}
