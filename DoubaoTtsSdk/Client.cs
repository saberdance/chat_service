namespace DoubaoTtsSdk
{
    using System;
    using System.Net.WebSockets;
    using System.Threading.Tasks;
    using System.Text.Json;
    using System.Text;
    using System.IO.Compression;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class Client
    {
        private readonly Dictionary<int, string> MessageTypes = new Dictionary<int, string>
        {
            {11, "audio-only server response"},
            {12, "frontend server response"},
            {15, "error message from server"}
        };

        private readonly Dictionary<int, string> MessageSerializationMethods = new Dictionary<int, string>
        {
            {0, "no serialization"},
            {1, "JSON"},
            {15, "custom type"}
        };

        private readonly Dictionary<int, string> MessageCompressions = new Dictionary<int, string>
        {
            {0, "no compression"},
            {1, "gzip"},
            {15, "custom compression method"}
        };

        private const string AppId = "9265231797";
        private const string Token = "noUrYSpjUDqbahLNmqaICvkvwcK1QKyT";
        private const string Cluster = "volcano_tts";
        private const string VoiceType = "BV700_V2_streaming";
        private const string Host = "openspeech.bytedance.com";
        private static readonly string ApiUrl = $"wss://{Host}/api/v1/tts/ws_binary";

        private static readonly byte[] DefaultHeader = new byte[] { 0x11, 0x10, 0x11, 0x00 };

        public async Task TestSubmit()
        {
            var requestJson = CreateRequestJson("submit");
            await SendRequest(requestJson, "test_submit.mp3");
        }

        public async Task TestQuery()
        {
            var requestJson = CreateRequestJson("query");
            await SendRequest(requestJson, "test_query.mp3");
        }

        private Dictionary<string, object> CreateRequestJson(string operation)
        {
            var requestJson = new Dictionary<string, object>
            {
                ["app"] = new Dictionary<string, string>
                {
                    ["appid"] = AppId,
                    ["token"] = "access_token",
                    ["cluster"] = Cluster
                },
                ["user"] = new Dictionary<string, string>
                {
                    ["uid"] = "1000000666"
                },
                ["audio"] = new Dictionary<string, object>
                {
                    ["voice_type"] = VoiceType,
                    ["encoding"] = "mp3",
                    ["speed_ratio"] = 1.0,
                    ["volume_ratio"] = 1.0,
                    ["pitch_ratio"] = 1.0,
                },
                ["request"] = new Dictionary<string, string>
                {
                    ["reqid"] = Guid.NewGuid().ToString(),
                    ["text"] = "大家好，我是四川万物数创科技有限公司的唐雪梅，我是一名项目经理，现在主要负责北京琉璃厂线上元宇宙的相关工作，谢谢大家的关心。",
                    ["text_type"] = "plain",
                    ["operation"] = operation
                }
            };

            return requestJson;
        }

        private async Task SendRequest(Dictionary<string, object> requestJson, string fileName)
        {
            var payloadBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(requestJson));
            payloadBytes = CompressPayload(payloadBytes);

            var fullClientRequest = new List<byte>(DefaultHeader);
            fullClientRequest.AddRange(BitConverter.GetBytes(payloadBytes.Length).Reverse()); // Big endian for payload size
            fullClientRequest.AddRange(payloadBytes);

            using (var ws = new ClientWebSocket())
            {
                ws.Options.AddSubProtocol("Tls");
                ws.Options.SetRequestHeader("Authorization", $"Bearer; {Token}");
                await ws.ConnectAsync(new Uri(ApiUrl), CancellationToken.None);

                await ws.SendAsync(new ArraySegment<byte>(fullClientRequest.ToArray()), WebSocketMessageType.Binary, true, CancellationToken.None);

                var buffer = new byte[4096];
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        ParseResponse(buffer.Take(result.Count).ToArray(), fileStream);
                    } while (!result.EndOfMessage);
                }

                Console.WriteLine("\nClosing the connection...");
            }
        }

        private byte[] CompressPayload(byte[] payload)
        {
            using (var compressedStream = new MemoryStream())
            using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                gzipStream.Write(payload, 0, payload.Length);
                gzipStream.Close();
                return compressedStream.ToArray();
            }
        }

        private void ParseResponse(byte[] response, FileStream fileStream)
        {
            Console.WriteLine("--------------------------- response ---------------------------");

            var protocolVersion = response[0] >> 4;
            var headerSize = response[0] & 0x0f;
            var messageType = response[1] >> 4;
            var messageCompression = response[2] & 0x0f;
            var payload = response.Skip(headerSize * 4).ToArray();

            Console.WriteLine($"Protocol version: {protocolVersion}");
            Console.WriteLine($"Header size: {headerSize * 4} bytes");
            Console.WriteLine($"Message type: {MessageTypes.GetValueOrDefault(messageType, "Unknown")}");
            Console.WriteLine($"Message compression: {MessageCompressions.GetValueOrDefault(messageCompression, "Unknown")}");

            // 根据消息类型处理响应
            switch (messageType)
            {
                case 11: // audio-only server response
                         // 如果消息被压缩，首先解压缩
                    if (messageCompression != 0)
                    {
                        payload = DecompressPayload(payload, messageCompression);
                    }
                    // 处理音频数据
                    fileStream.Write(payload, 0, payload.Length);
                    Console.WriteLine("Audio data processed.");
                    break;
                case 12: // frontend server response
                         // 如果消息被压缩，首先解压缩
                    if (messageCompression != 0)
                    {
                        payload = DecompressPayload(payload, messageCompression);
                    }
                    // 处理前端响应
                    var frontendMessage = Encoding.UTF8.GetString(payload);
                    Console.WriteLine($"Frontend response received: {frontendMessage}");
                    break;
                //case 15: // error message from server
                //         // 错误消息通常是文本格式，但也可能被压缩
                //    var errorMessage = Encoding.UTF8.GetString(DecompressPayload(payload, messageCompression));
                //    Console.WriteLine($"Error message: {errorMessage}");
                //    break;
                default:
                    Console.WriteLine("Undefined message type received.");
                    break;
            }
        }

        private byte[] DecompressPayload(byte[] payload, int compressionMethod)
        {
            switch (compressionMethod)
            {
                case 1: // gzip compression
                    using (var compressedStream = new MemoryStream(payload))
                    using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                    using (var decompressedStream = new MemoryStream())
                    {
                        gzipStream.CopyTo(decompressedStream);
                        return decompressedStream.ToArray();
                    }
                // 如果需要支持更多压缩方法，可以在这里添加更多的case分支
                default:
                    return payload; // 如果没有压缩或不支持的压缩方法，直接返回原始负载
            }
        }
    }
}
