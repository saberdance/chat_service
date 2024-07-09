namespace tts_service.Models.Protocol
{
    public class ChatRequest
    {
        public int user_id { get; set; }
        public int session_id { get; set; }
        //text voice
        public string? output_type { get; set; }
        public string? payload { get; set; }
    }
}
