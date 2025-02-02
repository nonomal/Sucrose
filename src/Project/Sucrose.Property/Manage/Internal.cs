using Wpf.Ui.Controls;
using SSDEPT = Sucrose.Shared.Dependency.Enum.PropertiesType;
using SSTHI = Sucrose.Shared.Theme.Helper.Info;
using SSTHP = Sucrose.Shared.Theme.Helper.Properties;

namespace Sucrose.Property.Manage
{
    internal static class Internal
    {
        public static SSTHI Info;

        public static string Path;

        public static string InfoPath;

        public static string WatcherFile;

        public static string PropertiesFile;

        public static string PropertiesPath;

        public static SSDEPT PropertiesType;

        public static string LibraryLocation;

        public static string LibrarySelected;

        public static string PropertiesCache;

        public static bool EngineLive = false;

        public static SSTHP Properties = new();

        public static WindowBackdropType DefaultBackdropType = WindowBackdropType.None;
    }
}