using Microsoft.AspNetCore.Mvc;

namespace tts_service.Models.Protocol
{
    public class TtsResponse : BaseResponse
    {
        public string? Audio { get; set; }
    }
}
