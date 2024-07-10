using DoubaoTtsSdk;
using Microsoft.Identity.Client;
using System.Diagnostics;
using tsubasa;
using tts_service.Models.Chat;
using tts_service.Models.Session;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace tts_service.Services.Chat.AIChater
{
    public class DoubaoChater : IChater
    {
        private string ChaterPath = "./tools/chater/doubao";
        private ChatJob _job;
        private ChatContent _outerContent;
        private string? _inputs;

        public async Task<ChatContent> Chat(ChatJob job, ChatContent outercontent,string inputs)
        {
            _job = job;
            _outerContent = outercontent;
            _inputs = inputs;
            var outputLines = new List<string>();
            var processStartInfo = new ProcessStartInfo
            {
                FileName = ChaterPath,
                Arguments = GenerateChatArgs(),
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (var process = Process.Start(processStartInfo))
            {
                using (var reader = process.StandardOutput)
                {
                    string line;
                    bool chunkStartFound = false;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("[CHUNK_END]"))
                        {
                            if (outputLines.Count>0)
                            {
                                await GenerateTtsJob(outputLines);
                            } 
                            break;
                        }
                        if (chunkStartFound)
                        {
                            outputLines.Add(line);
                            await _job.Client.SendAsync(line);
                        }
                        else if (line.Contains("[CHUNK_START]"))
                        {
                            chunkStartFound = true;
                            await _job.Client.SendAsync("[CHUNK_START]");
                        }
                    }
                }
            }

            return _outerContent;
        }

        private async Task GenerateTtsJob(List<string> aiAnswerLines)
        {
            ClientHttp client = new ClientHttp();
            await client.SendRequest(new()
            {
                user = new UserInfo()
                {
                    uid = _job.UserId.ToString()
                },
                request = new RequestInfo()
                {
                    text = GenTtsInputText(aiAnswerLines)
                }
            }, Defines.StaticVideosPath + "/" + UtilFunc.GetUnixTimeStamp(DateTime.Now).ToString()+"mp3");
        }

        private string GenTtsInputText(List<string> aiAnswerLines)
        {
            return string.Join("\n", aiAnswerLines);

        }

        private string GenerateChatArgs()
        {
            throw new NotImplementedException();
        }
    }
}
