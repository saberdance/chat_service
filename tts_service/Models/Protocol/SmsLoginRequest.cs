namespace tts_service.Models.Protocol
{
    public class SmsLoginRequest
    {
        public string? Phone { get; set; }
        public string? Code { get; set; }
    }
}
