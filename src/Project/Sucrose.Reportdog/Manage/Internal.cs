﻿using System.IO;
using SBHI = Sucrose.Backgroundog.Helper.Initialize;
using SMMRP = Sucrose.Memory.Manage.Readonly.Path;
using SMR = Sucrose.Memory.Readonly;
using Timer = System.Threading.Timer;

namespace Sucrose.Reportdog.Manage
{
    internal static class Internal
    {
        public static bool Exit = true;

        public static int AppTime = 1000;

        public static SBHI Initialize = new();

        public static int InitializeTime = 5000;

        public static Timer InitializeTimer = null;

        public static FileSystemWatcher Watcher = null;

        public static readonly string Source = Path.Combine(SMMRP.ApplicationData, SMR.AppName, SMR.CacheFolder, SMR.ReportFolder);
    }
}