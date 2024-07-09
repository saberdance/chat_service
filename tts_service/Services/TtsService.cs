using DoubaoTtsSdk;
using tts_service.Models.Stastics;
using tts_service.Models.TtsData;

namespace tts_service.Services
{
    public class TtsService
    {
        private TtsEngine _engine;
        private TtsVoice _voice;
        private TtsCall _call;
        private string? _filePath;
        private string? _text;

        public TtsService(TtsEngine engine, TtsVoice voice, string? text,string? filePath)
        {
            _engine = engine;
            _voice = voice;
            _text = text;
            _filePath = filePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>FilePath</returns>
        public async Task<string> TtsGen()
        {
            switch (_engine.Name)
            {
                case "doubao":
                    return await GenDoubaoTts();
                default:
                    return await Task.FromResult("");
            }
        }

        private async Task<string> GenDoubaoTts()
        {
            ClientHttp client = new ClientHttp();
            TtsRequest request = new()
            {
                user = new UserInfo()
                {
                    uid = DateTime.Now.ToString("MM:dd:yy:hh:mm:ss")
                },
                request = new RequestInfo()
                {
                    text = _text
                },
                audio = new AudioInfo()
                {
                    voice_type = _voice.Name
                }
            };
            await client.SendRequest(request, _filePath); 
            return _filePath;
        }
    }
}
