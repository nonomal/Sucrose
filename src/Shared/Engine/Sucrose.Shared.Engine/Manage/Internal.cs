﻿using System.Net;
using System.Windows.Threading;
using SSDSHS = Sucrose.Shared.Dependency.Struct.HandleStruct;
using SSESB = Sucrose.Shared.Engine.Setting.Browser;
using SSTHC = Sucrose.Shared.Theme.Helper.Compatible;
using SSTHI = Sucrose.Shared.Theme.Helper.Info;
using SSTHP = Sucrose.Shared.Theme.Helper.Properties;

namespace Sucrose.Shared.Engine.Manage
{
    internal static class Internal
    {
        public static SSTHI Info = new();

        public static bool IsDesktop = true;

        public static bool Interaction = true;

        public static SSTHC Compatible = new();

        public static bool Initialized = false;

        public static bool PauseVolume = false;

        public static SSTHP Properties = new();

        public static string Host = string.Empty;

        public static List<int> Processes = new();

        public static int IntermediateD3DWindow = 0;

        public static bool PausePerformance = false;

        public static bool PropertiesWatcher = true;

        public static string CpuData = string.Empty;

        public static string BiosData = string.Empty;

        public static string DateData = string.Empty;

        public static string InfoPath = string.Empty;

        public static string AudioData = string.Empty;

        public static string MemoryData = string.Empty;

        public static List<SSDSHS> Applications = new();

        public static string BatteryData = string.Empty;

        public static string NetworkData = string.Empty;

        public static string GraphicData = string.Empty;

        public static string WatcherFile = string.Empty;

        public static IntPtr WindowHandle = IntPtr.Zero;

        public delegate void ExecuteNormal(string Script);

        public static string CompatiblePath = string.Empty;

        public static string PropertiesFile = string.Empty;

        public static string PropertiesPath = string.Empty;

        public static string LibraryLocation = string.Empty;

        public static string LibrarySelected = string.Empty;

        public static string MotherboardData = string.Empty;

        public static string PropertiesCache = string.Empty;

        public static IPAddress Loopback = IPAddress.Loopback;

        public delegate Task<string> ExecuteTask(string script);

        public static CancellationTokenSource Displaying = new();

        public static readonly DispatcherTimer GeneralTimer = new();

        public static readonly string MpvProperties = "ew0KICAiUHJvcGVydHlMaXN0ZW5lciI6ICJTdWNyb3NlUHJvcGVydHlMaXN0ZW5lcignezB9JywgezF9KTsiLA0KICAiUHJvcGVydHlMaXN0Ijogew0KICAgICJvbmx5TXB2Ijogew0KICAgICAgInR5cGUiOiAibGFiZWwiLA0KICAgICAgInZhbHVlIjogIlByb3BlcnR5LkxvY2FsaXphdGlvbi5NcHZQbGF5ZXIiDQogICAgfSwNCiAgICAidmlkZW8tem9vbSI6IHsNCiAgICAgICJtYXgiOiA1LA0KICAgICAgIm1pbiI6IC01LA0KICAgICAgInN0ZXAiOiAwLjUsDQogICAgICAidGV4dCI6ICJQcm9wZXJ0eS5Mb2NhbGl6YXRpb24uWm9vbSIsDQogICAgICAidHlwZSI6ICJzbGlkZXIiLA0KICAgICAgInZhbHVlIjogMA0KICAgIH0sDQogICAgInNhdHVyYXRpb24iOiB7DQogICAgICAibWF4IjogMTAwLA0KICAgICAgIm1pbiI6IC0xMDAsDQogICAgICAic3RlcCI6IDEsDQogICAgICAidGV4dCI6ICJQcm9wZXJ0eS5Mb2NhbGl6YXRpb24uU2F0dXJhdGlvbiIsDQogICAgICAidHlwZSI6ICJzbGlkZXIiLA0KICAgICAgInZhbHVlIjogMA0KICAgIH0sDQogICAgImh1ZSI6IHsNCiAgICAgICJtYXgiOiAxMDAsDQogICAgICAibWluIjogLTEwMCwNCiAgICAgICJzdGVwIjogMSwNCiAgICAgICJ0ZXh0IjogIlByb3BlcnR5LkxvY2FsaXphdGlvbi5IdWUiLA0KICAgICAgInR5cGUiOiAic2xpZGVyIiwNCiAgICAgICJ2YWx1ZSI6IDANCiAgICB9LA0KICAgICJzaGFycGVuIjogew0KICAgICAgIm1heCI6IDEwMCwNCiAgICAgICJtaW4iOiAwLA0KICAgICAgInN0ZXAiOiAxLA0KICAgICAgInRleHQiOiAiUHJvcGVydHkuTG9jYWxpemF0aW9uLlNoYXJwbmVzcyIsDQogICAgICAidHlwZSI6ICJzbGlkZXIiLA0KICAgICAgInZhbHVlIjogMA0KICAgIH0sDQogICAgImJyaWdodG5lc3MiOiB7DQogICAgICAibWF4IjogMTAwLA0KICAgICAgIm1pbiI6IC0xMDAsDQogICAgICAic3RlcCI6IDEsDQogICAgICAidGV4dCI6ICJQcm9wZXJ0eS5Mb2NhbGl6YXRpb24uQnJpZ2h0bmVzcyIsDQogICAgICAidHlwZSI6ICJzbGlkZXIiLA0KICAgICAgInZhbHVlIjogMA0KICAgIH0sDQogICAgImNvbnRyYXN0Ijogew0KICAgICAgIm1heCI6IDEwMCwNCiAgICAgICJtaW4iOiAtMTAwLA0KICAgICAgInN0ZXAiOiAxLA0KICAgICAgInRleHQiOiAiUHJvcGVydHkuTG9jYWxpemF0aW9uLkNvbnRyYXN0IiwNCiAgICAgICJ0eXBlIjogInNsaWRlciIsDQogICAgICAidmFsdWUiOiAwDQogICAgfSwNCiAgICAiZ2FtbWEiOiB7DQogICAgICAibWF4IjogMTAwLA0KICAgICAgIm1pbiI6IC0xMDAsDQogICAgICAic3RlcCI6IDEsDQogICAgICAidGV4dCI6ICJQcm9wZXJ0eS5Mb2NhbGl6YXRpb24uR2FtbWEiLA0KICAgICAgInR5cGUiOiAic2xpZGVyIiwNCiAgICAgICJ2YWx1ZSI6IDANCiAgICB9LA0KICAgICJzcGVlZCI6IHsNCiAgICAgICJtYXgiOiAyLjUsDQogICAgICAibWluIjogMC4yNSwNCiAgICAgICJzdGVwIjogMC4xLA0KICAgICAgInRleHQiOiAiUHJvcGVydHkuTG9jYWxpemF0aW9uLlNwZWVkIiwNCiAgICAgICJ0eXBlIjogInNsaWRlciIsDQogICAgICAidmFsdWUiOiAxDQogICAgfSwNCiAgICAibXV0ZSI6IHsNCiAgICAgICJ0ZXh0IjogIlByb3BlcnR5LkxvY2FsaXphdGlvbi5NdXRlIiwNCiAgICAgICJ0eXBlIjogImNoZWNrYm94IiwNCiAgICAgICJ2YWx1ZSI6IGZhbHNlDQogICAgfQ0KICB9DQp9";

        public static SSESB BrowserSettings = new()
        {
            WebView = WebArguments,
            CefSharp = CefArguments
        };

        public static List<string> MpvConfig = new()
        {
            "# Video Settings #",
            "vo=gpu",
            "hwdec=no",
            "v-sync=no",
            "no-idle=no",
            "deband=yes",
            "fullscreen=yes",
            "hwdec-codecs=all",
            "interpolation=no",
            "scale-antiring=0",
            "framedrop=decoder",
            "temporal-dither=no",
            "profile=low-latency",
            "gpu-shader-cache=yes",
            "# Video Settings #",

            "",

            "# Audio Settings #",
            "volume-min=0",
            "volume-max=100",
            "audio-buffer=0.1",
            "audio-channels=stereo",
            "audio-pass-through=yes",
            "audio-samplerate=48000",
            "# Audio Settings #",

            "",

            "# General Performance Settings #",
            "cache=yes",
            "keep-open=no",
            "gpu-context=d3d11",
            "demuxer-max-bytes=64M",
            "video-sync=display-desync",
            "demuxer-seekable-cache=yes",
            "# General Performance Settings #"
        };

        public static List<string> WebArguments = new()
        {
            //"--enable-gpu",
            //"--enable-gpu-vsync",
            "--disable-gpu-compositing",

            "--disable-direct-write",
            //"--disable-frame-rate-limit",
            "--enable-begin-frame-scheduling",
            "--disable-breakpad",
            "--disable-extensions",
            "--disable-third-party-extensions",

            "--multi-threaded-message-loop",
            "--no-sandbox",

            "--disable-back-forward-cache",

            "--disable-web-security",
            "--disable-geolocation",
            "--disable-oor-cors",

            "--disable-surfaces",

            "--autoplay-policy=no-user-gesture-required",

            "--enable-media-stream",
            "--enable-accelerated-video-decode",

            "--allow-running-insecure-content",
            "--use-fake-ui-for-media-stream",
            "--enable-usermedia-screen-capture",
            "--enable-usermedia-screen-capturing",
            "--debug-plugin-loading",
            "--allow-outdated-plugins",
            "--always-authorize-plugins",
            "--enable-npapi",

            "--disable-speech-input",

            "--allow-file-access",
            "--allow-file-access-from-files",
            "--allow-file-access-from-file-urls",
            "--allow-universal-access-from-files",

            "--unsafely-disable-devtools-self-xss-warnings",

            "--disable-site-isolation-trials",
            "--disable-blink-features=BlockCredentialedSubresources",

            "--disable-features=MediaEngagementBypassAutoplayPolicies,CrossSiteDocumentBlockingIfIsolating,BlockInsecurePrivateNetworkRequests,CrossSiteDocumentBlockingAlways,PreloadMediaEngagementData,OutOfBlinkCors,IsolateOrigins"
        };

        public static Dictionary<string, string> CefArguments = new()
        {
            //{ "enable-gpu", "1" },
            //{ "enable-gpu-vsync", "1" },
            { "disable-gpu-compositing", "1" },

            { "disable-direct-write", "1" },
            //{ "disable-frame-rate-limit", "1" },
            { "enable-begin-frame-scheduling", "1" },
            { "disable-breakpad", "1" },
            { "disable-extensions", "1" },
            { "disable-third-party-extensions", "1" },

            { "multi-threaded-message-loop", "1" },
            { "no-sandbox", "1" },
            { "off-screen-rendering-enabled", "1" },

            { "disable-back-forward-cache", "1" },

            { "disable-web-security", "1" },
            { "disable-geolocation", "1" },
            { "disable-oor-cors", "1" },

            { "disable-surfaces", "1" },

            { "autoplay-policy", "no-user-gesture-required" },

            { "enable-media-stream", "1" },
            { "enable-accelerated-video-decode", "1" },

            { "allow-running-insecure-content", "1" },
            { "use-fake-ui-for-media-stream", "1" },
            { "enable-usermedia-screen-capture", "1" },
            { "enable-usermedia-screen-capturing", "1" },
            { "debug-plugin-loading", "1" },
            { "allow-outdated-plugins", "1" },
            { "always-authorize-plugins", "1" },
            { "enable-npapi", "1" },

            { "disable-speech-input", "1" },

            { "allow-file-access", "1" },
            { "allow-file-access-from-files", "1" },
            { "allow-file-access-from-file-urls", "1" },
            { "allow-universal-access-from-files", "1" },

            { "unsafely-disable-devtools-self-xss-warnings", "1" },

            { "disable-site-isolation-trials", "1" },
            { "disable-blink-features", "BlockCredentialedSubresources" },

            { "disable-features", "MediaEngagementBypassAutoplayPolicies,CrossSiteDocumentBlockingIfIsolating,BlockInsecurePrivateNetworkRequests,CrossSiteDocumentBlockingAlways,PreloadMediaEngagementData,OutOfBlinkCors,IsolateOrigins" }
        };
    }
}