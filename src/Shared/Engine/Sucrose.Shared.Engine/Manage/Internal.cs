using System.Net;
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

        public static readonly string MpvProperties = "ew0KICAiUHJvcGVydHlMaXN0ZW5lciI6ICJTdWNyb3NlUHJvcGVydHlMaXN0ZW5lcignezB9JywgezF9KTsiLA0KICAiUHJvcGVydHlMaXN0Ijogew0KICAgICJvbmx5TXB2Ijogew0KICAgICAgInR5cGUiOiAibGFiZWwiLA0KICAgICAgInZhbHVlIjogImxvY19vbmx5X21wdiINCiAgICB9LA0KICAgICJzYXR1cmF0aW9uIjogew0KICAgICAgIm1heCI6IDEwMCwNCiAgICAgICJtaW4iOiAtMTAwLA0KICAgICAgInN0ZXAiOiAxLA0KICAgICAgInRleHQiOiAibG9jX3NhdHVyYXRpb24iLA0KICAgICAgInR5cGUiOiAic2xpZGVyIiwNCiAgICAgICJ2YWx1ZSI6IDANCiAgICB9LA0KICAgICJodWUiOiB7DQogICAgICAibWF4IjogMTAwLA0KICAgICAgIm1pbiI6IC0xMDAsDQogICAgICAic3RlcCI6IDEsDQogICAgICAidGV4dCI6ICJsb2NfaHVlIiwNCiAgICAgICJ0eXBlIjogInNsaWRlciIsDQogICAgICAidmFsdWUiOiAwDQogICAgfSwNCiAgICAiYnJpZ2h0bmVzcyI6IHsNCiAgICAgICJtYXgiOiAxMDAsDQogICAgICAibWluIjogLTEwMCwNCiAgICAgICJzdGVwIjogMSwNCiAgICAgICJ0ZXh0IjogImxvY19icmlnaHRuZXNzIiwNCiAgICAgICJ0eXBlIjogInNsaWRlciIsDQogICAgICAidmFsdWUiOiAwDQogICAgfSwNCiAgICAiY29udHJhc3QiOiB7DQogICAgICAibWF4IjogMTAwLA0KICAgICAgIm1pbiI6IC0xMDAsDQogICAgICAic3RlcCI6IDEsDQogICAgICAidGV4dCI6ICJsb2NfY29udHJhc3QiLA0KICAgICAgInR5cGUiOiAic2xpZGVyIiwNCiAgICAgICJ2YWx1ZSI6IDANCiAgICB9LA0KICAgICJnYW1tYSI6IHsNCiAgICAgICJtYXgiOiAxMDAsDQogICAgICAibWluIjogLTEwMCwNCiAgICAgICJzdGVwIjogMSwNCiAgICAgICJ0ZXh0IjogImxvY19nYW1tYSIsDQogICAgICAidHlwZSI6ICJzbGlkZXIiLA0KICAgICAgInZhbHVlIjogMA0KICAgIH0sDQogICAgInNwZWVkIjogew0KICAgICAgIm1heCI6IDIuNSwNCiAgICAgICJtaW4iOiAwLjI1LA0KICAgICAgInN0ZXAiOiAwLjEsDQogICAgICAidGV4dCI6ICJsb2Nfc3BlZWQiLA0KICAgICAgInR5cGUiOiAic2xpZGVyIiwNCiAgICAgICJ2YWx1ZSI6IDENCiAgICB9LA0KICAgICJtdXRlIjogew0KICAgICAgInRleHQiOiAibG9jX211dGUiLA0KICAgICAgInR5cGUiOiAiY2hlY2tib3giLA0KICAgICAgInZhbHVlIjogZmFsc2UNCiAgICB9DQogIH0sDQogICJQcm9wZXJ0eUxvY2FsaXphdGlvbiI6IHsNCiAgICAiRU4iOiB7DQogICAgICAibG9jX29ubHlfbXB2IjogIk9ubHkgZm9yIE1wdlBsYXllciIsDQogICAgICAibG9jX3NhdHVyYXRpb24iOiAiU2F0dXJhdGlvbiIsDQogICAgICAibG9jX2h1ZSI6ICJIdWUiLA0KICAgICAgImxvY19icmlnaHRuZXNzIjogIkJyaWdodG5lc3MiLA0KICAgICAgImxvY19jb250cmFzdCI6ICJDb250cmFzdCIsDQogICAgICAibG9jX2dhbW1hIjogIkdhbW1hIiwNCiAgICAgICJsb2Nfc3BlZWQiOiAiU3BlZWQiLA0KICAgICAgImxvY19tdXRlIjogIk11dGUiDQogICAgfSwNCiAgICAiREUiOiB7DQogICAgICAibG9jX29ubHlfbXB2IjogIk51ciBmw7xyIE1wdlBsYXllciIsDQogICAgICAibG9jX3NhdHVyYXRpb24iOiAiU8OkdHRpZ3VuZyIsDQogICAgICAibG9jX2h1ZSI6ICJGYXJidG9uIiwNCiAgICAgICJsb2NfYnJpZ2h0bmVzcyI6ICJIZWxsaWdrZWl0IiwNCiAgICAgICJsb2NfY29udHJhc3QiOiAiS29udHJhc3QiLA0KICAgICAgImxvY19nYW1tYSI6ICJHYW1tYSIsDQogICAgICAibG9jX3NwZWVkIjogIkdlc2Nod2luZGlna2VpdCIsDQogICAgICAibG9jX211dGUiOiAiU3R1bW1zY2hhbHRlbiINCiAgICB9LA0KICAgICJFUyI6IHsNCiAgICAgICJsb2Nfb25seV9tcHYiOiAiU8OzbG8gcGFyYSBNcHZQbGF5ZXIiLA0KICAgICAgImxvY19zYXR1cmF0aW9uIjogIlNhdHVyYWNpw7NuIiwNCiAgICAgICJsb2NfaHVlIjogIk1hdGl6IiwNCiAgICAgICJsb2NfYnJpZ2h0bmVzcyI6ICJCcmlsbG8iLA0KICAgICAgImxvY19jb250cmFzdCI6ICJDb250cmFzdGUiLA0KICAgICAgImxvY19nYW1tYSI6ICJHYW1tYSIsDQogICAgICAibG9jX3NwZWVkIjogIlZlbG9jaWRhZCIsDQogICAgICAibG9jX211dGUiOiAiU2lsZW5jaWFyIg0KICAgIH0sDQogICAgIkZSIjogew0KICAgICAgImxvY19vbmx5X21wdiI6ICJVbmlxdWVtZW50IHBvdXIgTXB2UGxheWVyIiwNCiAgICAgICJsb2Nfc2F0dXJhdGlvbiI6ICJTYXR1cmF0aW9uIiwNCiAgICAgICJsb2NfaHVlIjogIlRlaW50ZSIsDQogICAgICAibG9jX2JyaWdodG5lc3MiOiAiTHVtaW5vc2l0w6kiLA0KICAgICAgImxvY19jb250cmFzdCI6ICJDb250cmFzdGUiLA0KICAgICAgImxvY19nYW1tYSI6ICJHYW1tYSIsDQogICAgICAibG9jX3NwZWVkIjogIlZpdGVzc2UiLA0KICAgICAgImxvY19tdXRlIjogIk11ZXQiDQogICAgfSwNCiAgICAiUEwiOiB7DQogICAgICAibG9jX29ubHlfbXB2IjogIlR5bGtvIGRsYSBNcHZQbGF5ZXJhIiwNCiAgICAgICJsb2Nfc2F0dXJhdGlvbiI6ICJOYXN5Y2VuaWUiLA0KICAgICAgImxvY19odWUiOiAiT2RjaWXFhCIsDQogICAgICAibG9jX2JyaWdodG5lc3MiOiAiSmFzbm/Fm8SHIiwNCiAgICAgICJsb2NfY29udHJhc3QiOiAiS29udHJhc3QiLA0KICAgICAgImxvY19nYW1tYSI6ICJHYW1tYSIsDQogICAgICAibG9jX3NwZWVkIjogIlByxJlka2/Fm8SHIiwNCiAgICAgICJsb2NfbXV0ZSI6ICJXeWNpc3oiDQogICAgfSwNCiAgICAiVFIiOiB7DQogICAgICAibG9jX29ubHlfbXB2IjogIlNhZGVjZSBNcHZQbGF5ZXIgxLDDp2luIiwNCiAgICAgICJsb2Nfc2F0dXJhdGlvbiI6ICJEb3lndW5sdWsiLA0KICAgICAgImxvY19odWUiOiAiUmVuayBUb251IiwNCiAgICAgICJsb2NfYnJpZ2h0bmVzcyI6ICJQYXJsYWtsxLFrIiwNCiAgICAgICJsb2NfY29udHJhc3QiOiAiS29udHJhc3QiLA0KICAgICAgImxvY19nYW1tYSI6ICJHYW1hIiwNCiAgICAgICJsb2Nfc3BlZWQiOiAiSMSxeiIsDQogICAgICAibG9jX211dGUiOiAiU2Vzc2l6Ig0KICAgIH0NCiAgfQ0KfQ==";

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
            "gpu-context=win",
            "demuxer-max-bytes=64M",
            "demuxer-seekable-cache=no",
            "video-sync=display-desync",
            "# General Performance Settings #"
        };

        public static List<string> WebArguments = new()
        {
            //"--enable-gpu",
            //"--enable-gpu-vsync",
            //"--disable-gpu-compositing",

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
            //{ "disable-gpu-compositing", "1" },

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