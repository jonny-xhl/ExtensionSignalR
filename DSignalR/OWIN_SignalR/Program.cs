using Microsoft.Owin.Hosting;
using System;
using System.Net;
using System.Net.NetworkInformation;

namespace OWIN_SignalR
{
    class Program
    {
        //参考：http://www.asp.net/signalr/overview/deployment/tutorial-signalr-self-host
        //注意：如果执行失败，除了发生System.Reflection.TargetInvocationException 错误，你需要以管理员权限重新运行VS        
        static void Main(string[] args)
        {
            // 这将 *ONLY* 绑定到 localhost，如果你想要绑定到的所有地址。使用 http://*:8080 去绑定所以地址
            // 更多： http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 

            //端口不固定，只要这个端口没用被使用就行
            //var iPAddresses=Dns.GetHostAddresses("localhost");
            int port = 5438, @is = 0;
            if (IPEndPoint.MinPort <= port & IPEndPoint.MaxPort >= port)
            {
                IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] iPEndPoints = iPGlobalProperties.GetActiveTcpListeners();
                foreach (IPEndPoint item in iPEndPoints)
                {
                    if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && item.Port == port)
                    {
                        @is = 1;
                        break;
                    }
                }
                if (@is == 0)
                {
                    string url = $"http://localhost:{port}";
                    // 启动 OWIN Host
                    using (WebApp.Start(url))
                    {
                        Console.WriteLine("Server running on {0}", url);
                        Console.ReadLine();
                    }
                }
                else
                {
                    throw new HttpListenerException(14007, $"端口：【{port}】已被占用，请换一个端口再试！");
                }
            }
            else
            {
                throw new HttpListenerException(14007, $"端口：【{port}】不是有效的端口，端口在0-65535之间！");
            }
        }
    }
}