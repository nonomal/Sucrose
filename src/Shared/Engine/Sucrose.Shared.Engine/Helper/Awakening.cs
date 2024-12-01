using ESFLAGS = Skylark.Wing.Native.Methods.ES_FLAGS;
using SMME = Sucrose.Manager.Manage.Engine;
using SSWEW = Sucrose.Shared.Watchdog.Extension.Watch;
using SWNM = Skylark.Wing.Native.Methods;
using Timer = System.Timers.Timer;

namespace Sucrose.Shared.Engine.Helper
{
    internal static class Awakening
    {
        public static void Start()
        {
            int Second = 30;

            Timer Awaker = new(Second * 1000);

            Awaker.Elapsed += async (s, e) =>
            {
                try
                {
                    if (SMME.StayAwake)
                    {
                        SWNM.SetThreadExecutionState(ESFLAGS.ES_CONTINUOUS | ESFLAGS.ES_SYSTEM_REQUIRED | ESFLAGS.ES_DISPLAY_REQUIRED);
                    }
                    else
                    {
                        SWNM.SetThreadExecutionState(ESFLAGS.ES_CONTINUOUS);
                    }
                }
                catch (Exception Exception)
                {
                    await SSWEW.Watch_CatchException(Exception);
                }
            };

            Awaker.AutoReset = true;

            Awaker.Start();
        }
    }
}