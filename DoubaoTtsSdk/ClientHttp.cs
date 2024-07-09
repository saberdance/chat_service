using Microsoft.Extensions.DiagnosticAdapter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using tsubasa;

namespace DoubaoTtsSdk
{
    public class TtsRequest:DumpableClass
    {
        public AppInfo app { get; set; } = new AppInfo();
        public UserInfo user { get; set; } = new UserInfo();
        public AudioInfo audio { get; set; } = new AudioInfo();
        public RequestInfo request { get; set; } = new RequestInfo();
    }

    public class AppInfo: DumpableClass
    {
        public string appid { get; set; } = "9265231797";
        public string token { get; set; } = "noUrYSpjUDqbahLNmqaICvkvwcK1QKyT";
        public string cluster { get; set; } = "volcano_tts";
    }

    public class UserInfo: DumpableClass
    {
        public string uid { get; set; } = "1";
    }

    public class AudioInfo: DumpableClass
    {
        public string voice_type { get; set; } = "BV700_V2_streaming";
        public string encoding { get; set; } = "mp3";
        public double speed_ratio { get; set; } = 1.0;
        public double volume_ratio { get; set; } = 1.0;
        public double pitch_ratio { get; set; } = 1.0;
    }

    public class RequestInfo: DumpableClass
    {
        public string reqid { get; set; } = Guid.NewGuid().ToString();
        public string text { get; set; }
        public string text_type { get; set; } = "plain";
        public string operation { get; set; } = "query";
        public int with_frontend { get; set; } = 1;
        public string frontend_type { get; set; } = "unitTson"; 
    }



    public class ClientHttp
    {
        private const string Host = "openspeech.bytedance.com";
        private const string ApiUrl = $"https://{Host}/api/v1/tts";
        public async Task SendRequest(TtsRequest request,string outputFile = "test_submit.mp3")
        {
            request.Dump();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer;{request.app.token}");
                    var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(ApiUrl, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"resp body: \n{responseString}");

                    var responseData = JsonSerializer.Deserialize<JsonElement>(responseString);
                    if (responseData.TryGetProperty("data", out var data))
                    {
                        File.WriteAllBytes(outputFile, Convert.FromBase64String(data.GetString()));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }
    }
}
