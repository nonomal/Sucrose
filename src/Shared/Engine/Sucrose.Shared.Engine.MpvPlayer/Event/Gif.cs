using System.IO;
using Application = System.Windows.Application;
using SSEHP = Sucrose.Shared.Engine.Helper.Properties;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;
using SSEMPHP = Sucrose.Shared.Engine.MpvPlayer.Helper.Properties;
using SSEMPMI = Sucrose.Shared.Engine.MpvPlayer.Manage.Internal;
using SSTHP = Sucrose.Shared.Theme.Helper.Properties;
using SSWEW = Sucrose.Shared.Watchdog.Extension.Watch;

namespace Sucrose.Shared.Engine.MpvPlayer.Event
{
    internal static class Gif
    {
        public static void MediaEngineMediaLoaded(object sender, EventArgs e)
        {
            SSEMI.Initialized = true;

            if (!string.IsNullOrEmpty(SSEMI.PropertiesFile))
            {
                SSEMI.Properties = SSTHP.ReadJson(SSEMI.PropertiesFile);
                SSEMI.Properties.State = true;
            }

            if (SSEMI.Properties.State)
            {
                if (SSEMI.PropertiesWatcher)
                {
                    SSEHP.CreatedEventHandler += PropertiesWatcher;
                }

                SSEHP.StartWatcher();

                SSEHP.ExecuteNormal(SSEMPHP.ExecuteScript);
            }
        }

        private static async void PropertiesWatcher(object sender, FileSystemEventArgs e)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                try
                {
                    SSEMI.Properties = SSTHP.ReadJson(e.FullPath);

                    if (SSEMPMI.MediaEngine != null && SSEMPMI.MediaEngine.IsMediaLoaded)
                    {
                        SSEHP.ExecuteNormal(SSEMPHP.ExecuteScript);
                    }
                }
                catch (Exception Exception)
                {
                    await SSWEW.Watch_CatchException(Exception);
                }
            });
        }
    }
}