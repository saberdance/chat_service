namespace tts_service.Models.Protocol
{
    public class BaseResponse<T>
    {
        public ProtocolErrorCode Code { get; set; } = ProtocolErrorCode.Success;
        public string? Message { get; set; } = "";
        public T? Data { get; set; }
    }
}
