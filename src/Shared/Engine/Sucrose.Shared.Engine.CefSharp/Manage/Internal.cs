using CefSharp;
using CefEngine = CefSharp.Wpf.HwndHost.ChromiumWebBrowser;

namespace Sucrose.Shared.Engine.CefSharp.Manage
{
    internal static class Internal
    {
        public static int Try = 0;

        public static string CefPath;

        public static bool State = true;

        public static CefEngine CefEngine = null;

        public static IBrowserHost CefHost = null;

        public static IntPtr CefHandle = IntPtr.Zero;

        public static BrowserSettings CefSettings => new()
        {
            WindowlessFrameRate = 60,
            BackgroundColor = Cef.ColorSetARGB(255, 0, 0, 0)
        };
    }
}