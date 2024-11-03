using System.Net;
using System.Net.Sockets;

namespace Sucrose.Shared.Space.Helper
{
    internal static class Port
    {
        public static int Available(IPAddress Host, int Default = 65432)
        {
            try
            {
                TcpListener Listener = new(Host, 0);

                Listener.Start();

                int Port = ((IPEndPoint)Listener.LocalEndpoint).Port;

                Listener.Stop();

                return Port;
            }
            catch
            {
                return Default;
            }
        }
    }
}