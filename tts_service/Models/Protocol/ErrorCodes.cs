namespace tts_service.Models.Protocol
{
    public enum ProtocolErrorCode
    {
        Success = 0,
        InvalidParameter = 1,
        InvalidCredential = 2,
        InvalidToken = 3,
        TokenExpired = 4,
        UserNotFound = 5,
        WebsocketError = 6,
    }
}
