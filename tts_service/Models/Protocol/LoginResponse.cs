namespace tts_service.Models.Protocol
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string? Guid { get; set; }
        public string? Token { get; set; }
        public DateTime TokenExpire { get; set; }
    }
}
