﻿using System.IO;
using SMMRA = Sucrose.Memory.Manage.Readonly.App;
using SMMRF = Sucrose.Memory.Manage.Readonly.Folder;
using SMMRG = Sucrose.Memory.Manage.Readonly.General;
using SMMRP = Sucrose.Memory.Manage.Readonly.Path;
using SSDEACT = Sucrose.Shared.Dependency.Enum.ArgumentCommandType;
using SSLHK = Sucrose.Shared.Live.Helper.Kill;
using SSSHF = Sucrose.Shared.Space.Helper.Filing;
using SSSHP = Sucrose.Shared.Space.Helper.Processor;
using SSSMI = Sucrose.Shared.Space.Manage.Internal;

namespace Sucrose.Shared.Space.Helper
{
    internal static class Import
    {
        public static async Task Start(string Destination, string Application)
        {
            SSLHK.Stop();

            SSSHP.Kill(SMMRA.Undo);
            SSSHP.Kill(SMMRA.Portal);
            SSSHP.Kill(SMMRA.Update);
            SSSHP.Kill(SMMRA.Launcher);
            SSSHP.Kill(SMMRA.Property);
            SSSHP.Kill(SMMRA.Watchdog);
            SSSHP.Kill(SMMRA.Reportdog);
            SSSHP.Kill(SMMRA.Backgroundog);

            await Task.Delay(TimeSpan.FromSeconds(3));

            if (!Directory.Exists(Destination))
            {
                Directory.CreateDirectory(Destination);
            }

            if (!Directory.Exists(Path.Combine(SMMRP.ApplicationData, SMMRG.AppName, SMMRF.Setting)))
            {
                Directory.CreateDirectory(Path.Combine(SMMRP.ApplicationData, SMMRG.AppName, SMMRF.Setting));
            }

            foreach (string Setting in Settings(Destination))
            {
                SSSHF.CopyBuffer(Setting, Path.Combine(SMMRP.ApplicationData, SMMRG.AppName, SMMRF.Setting, Path.GetFileName(Setting)));
            }

            await Task.Delay(TimeSpan.FromSeconds(1));

            SSSHP.Run(SSSMI.Portal, $"{SSDEACT.SystemSetting}");
            SSSHP.Run(Application);

            await Task.CompletedTask;
        }

        public static string[] Settings(string Destination)
        {
            try
            {
                return Directory.GetFiles(Destination, "*.json", SearchOption.TopDirectoryOnly);
            }
            catch
            {
                return Array.Empty<string>();
            }
        }
    }
}