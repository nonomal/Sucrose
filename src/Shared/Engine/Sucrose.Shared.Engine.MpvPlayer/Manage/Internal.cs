using System.IO;
using MediaEngine = Sucrose.Mpv.NET.Player.MpvPlayer;
using SSSMI = Sucrose.Shared.Space.Manage.Internal;

namespace Sucrose.Shared.Engine.MpvPlayer.Manage
{
    internal static class Internal
    {
        public static string Source;

        public static MediaEngine MediaEngine;

        public static readonly string LibPath = Path.Combine(SSSMI.This, "lib");

        public static readonly string MpvConfig = Path.Combine(LibPath, "mpv.conf");

#if X86
        public static readonly string MediaPath = Path.Combine(LibPath, "libmpv-x86.dll");
#elif X64
        public static readonly string MediaPath = Path.Combine(LibPath, "libmpv-x64.dll");
#else
        public static readonly string MediaPath = Path.Combine(LibPath, "libmpv-ARM64.dll");
#endif

    }
}