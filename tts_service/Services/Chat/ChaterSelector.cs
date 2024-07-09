using tts_service.Services.Chat.AIChater;

namespace tts_service.Services.Chat
{
    public static class ChaterSelector
    {
        public static IChater GetChater(string chaterName)
        {
            return chaterName switch
            {
                "doubao" => new DoubaoChater(),
                _ => new DoubaoChater()
            };
        }
    }
}
