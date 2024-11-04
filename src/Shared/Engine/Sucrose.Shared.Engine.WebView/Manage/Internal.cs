using WebEngine = Microsoft.Web.WebView2.Wpf.WebView2;

namespace Sucrose.Shared.Engine.WebView.Manage
{
    internal static class Internal
    {
        public static int Try = 0;

        public static bool State = true;

        public static IntPtr WebHandle = IntPtr.Zero;

        public static WebEngine WebEngine = new()
        {
            DefaultBackgroundColor = Color.Black
        };
    }
}