using CefSharp;
using System.Windows;
using SMME = Sucrose.Manager.Manage.Engine;
using SSECSHV = Sucrose.Shared.Engine.CefSharp.Helper.Video;
using SSECSMI = Sucrose.Shared.Engine.CefSharp.Manage.Internal;
using SSEHS = Sucrose.Shared.Engine.Helper.Source;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;

namespace Sucrose.Shared.Engine.CefSharp.Event
{
    internal static class Video
    {
        public static void CefEngineLoaded(object sender, RoutedEventArgs e)
        {
            Uri Video = SSEHS.GetSource(SSEMI.Info.Source, SSEMI.Host);

            string Path = SSEHS.GetVideoContentPath();

            SSEHS.WriteVideoContent(Path, Video);

            SSECSMI.CefEngine.Address = SSEHS.GetSource(Path).ToString();
        }

        public static void CefEngineInitializedChanged(object sender, EventArgs e)
        {
            if (SMME.DeveloperMode)
            {
                SSECSMI.CefEngine.ShowDevTools();
            }

            SSEMI.Initialized = SSECSMI.CefEngine.IsBrowserInitialized;
        }

        public static void CefEngineFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            SSECSHV.Load();
        }
    }
}