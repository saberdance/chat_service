namespace tts_service.Models.Protocol
{
    public class LoginResponse : BaseResponse
    {
        public string? Token { get; set; }
        public DateTime TokenExpire { get; set; }
    }
}
