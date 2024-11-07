using SSEVMI = Sucrose.Shared.Engine.Vexana.Manage.Internal;
using SSWEW = Sucrose.Shared.Watchdog.Extension.Watch;

namespace Sucrose.Shared.Engine.Vexana.Helper
{
    internal static class Gif
    {
        public static async void Play()
        {
            try
            {
                SSEVMI.ImageState = true;
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        public static async void Pause()
        {
            try
            {
                SSEVMI.ImageState = false;
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }

        public static async void SetLoop(bool State)
        {
            try
            {
                SSEVMI.ImageLoop = State;
            }
            catch (Exception Exception)
            {
                await SSWEW.Watch_CatchException(Exception);
            }
        }
    }
}