using DoubaoTtsSdk;
using System.Diagnostics.Metrics;

namespace tester
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ClientHttp client = new ClientHttp();
            await client.SendRequest(new()
            {
                user = new UserInfo()
                {
                    uid = DateTime.Now.ToString("MM:dd:yy:hh:mm:ss")
                },
                request = new RequestInfo()
                {
                    text = "大家好，我是四川山海互动的杨旭，我是一名资深美术设计师，现在主要负责王者荣耀的相关工作，谢谢大家的关心。"
                }
            }); 
            while (true)
            {
                await Task.Delay(1000);
            }
        }
    }
}
