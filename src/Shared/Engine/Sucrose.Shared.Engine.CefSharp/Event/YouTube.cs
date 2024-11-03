using CefSharp;
using System.Windows;
using SMME = Sucrose.Manager.Manage.Engine;
using SMMRC = Sucrose.Memory.Manage.Readonly.Content;
using SSECSHYT = Sucrose.Shared.Engine.CefSharp.Helper.YouTube;
using SSECSMI = Sucrose.Shared.Engine.CefSharp.Manage.Internal;
using SSEHS = Sucrose.Shared.Engine.Helper.Source;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;
using SSTHV = Sucrose.Shared.Theme.Helper.Various;

namespace Sucrose.Shared.Engine.CefSharp.Event
{
    internal static class YouTube
    {
        public static void CefEngineLoaded(object sender, RoutedEventArgs e)
        {
            string Video = SSTHV.GetYouTubeVideoId(SSEMI.Info.Source);
            string Playlist = SSTHV.GetYouTubePlaylistId(SSEMI.Info.Source);

            string Path = SSEHS.GetYouTubeContentPath();

            SSEHS.WriteYouTubeContent(Path, Video, Playlist);

            SSECSMI.CefEngine.Address = SSEHS.GetSource(SMMRC.YouTube, SSEMI.Host).ToString();
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
            SSECSHYT.Load();
        }
    }
}