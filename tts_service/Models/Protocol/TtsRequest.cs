﻿namespace tts_service.Models.Protocol
{
    public class TtsRequest
    {
        public string? UserId { get; set; }
        public string? Text { get; set; }
        public string? Voice { get; set; }
        public string? Engine { get; set; }
    }
}
