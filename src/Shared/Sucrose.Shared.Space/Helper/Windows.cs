using SWEOS = Skylark.Wing.Extension.OperatingSystem;

namespace Sucrose.Shared.Space.Helper
{
    internal static class Windows
    {
        public static bool IsCobalt(int Build = 22000)
        {
            return SWEOS.BuildNumber >= Build;
        }

        public static bool IsNickel(int Build = 22621) //22631
        {
            return SWEOS.BuildNumber >= Build;
        }

        public static bool IsGermanium(int Build = 26000) //26100
        {
            return SWEOS.BuildNumber >= Build;
        }

        public static bool IsRedstone1(int Build = 14393)
        {
            return SWEOS.BuildNumber >= Build;
        }

        public static bool IsVibranium(int Build = 19041)
        {
            return SWEOS.BuildNumber >= Build;
        }
    }
}